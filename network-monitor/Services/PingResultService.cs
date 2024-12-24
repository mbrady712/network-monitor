using Npgsql;
using System;
using System.Collections.Generic;
using network_monitor.Models;

namespace network_monitor.Services
{
    public class PingResultService
    {
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=network_monitoring;Username=postgres;Password=Kcml2121!";

        public void SavePingResult(PingResultModel pingResult)
        {
            if (!System.Net.IPAddress.TryParse(pingResult.IPAddress, out _))
            {
                throw new ArgumentException("Invalid IP address format.");
            }

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO ping_results (ip_address, success, status, round_trip_time, error_message, timestamp) VALUES (@ip, @success, @status, @rtt, @error, @timestamp)";
                    cmd.Parameters.AddWithValue("ip", System.Net.IPAddress.Parse(pingResult.IPAddress));
                    cmd.Parameters.AddWithValue("success", pingResult.Success);
                    cmd.Parameters.AddWithValue("status", pingResult.Status);
                    cmd.Parameters.AddWithValue("rtt", pingResult.RoundTripTime);
                    cmd.Parameters.AddWithValue("error", (object)pingResult.ErrorMessage ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("timestamp", DateTime.UtcNow);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PingResultModel> GetPingResults()
        {
            var results = new List<PingResultModel>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT ip_address, success, status, round_trip_time, error_message, timestamp FROM ping_results ORDER BY timestamp DESC", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new PingResultModel
                        {
                            IPAddress = reader.GetFieldValue<NpgsqlTypes.NpgsqlInet>(0).ToString(),
                            Success = reader.GetBoolean(1),
                            Status = reader.GetString(2),
                            RoundTripTime = reader.IsDBNull(3) ? 0 : reader.GetInt64(3),
                            ErrorMessage = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Timestamp = reader.GetDateTime(5)
                        });
                    }
                }
            }

            return results;
        }
    }
}
