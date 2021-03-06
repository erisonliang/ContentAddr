﻿using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Lokad.ContentAddr.Tests
{
    public sealed class checksum
    {
        [Fact]
        public void CRC32_sample_value()
        {
            var buffer = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };

            var crc = Checksum.CRC32(buffer, 0, 9);
            // See http://www.sunshine2k.de/coding/javascript/crc/crc_js.html
            // See http://www.lammertbies.nl/comm/info/crc-calculation.html
            Assert.Equal("CBF43926", crc.ToString("X8"));
        }


        [Fact]
        public void CRC32_sample_value_rospan()
        {
            var buffer = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };

            var crc = Checksum.CRC32(buffer.AsSpan(0, 9));
            // See http://www.sunshine2k.de/coding/javascript/crc/crc_js.html
            // See http://www.lammertbies.nl/comm/info/crc-calculation.html
            Assert.Equal("CBF43926", crc.ToString("X8"));
        }

        [Fact]
        public void CRC32_sample_value_stream()
        {
            var buffer = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };
            using (var stream = new MemoryStream(buffer))
            {
                Assert.Equal("CBF43926", 
                    Checksum.CRC32(stream, 0, buffer.Length).ToString("X8"));
            }
        }

        [Fact]
        public void CRC32_sample_value_stream_buffered()
        {
            var buffer = Enumerable.Range(0, 10000000).Select(i => (byte)i).ToArray();
            using (var stream = new MemoryStream(buffer))
            {
                Assert.Equal("14BFFAE4",
                    Checksum.CRC32(stream, 0, buffer.Length).ToString("X8"));
            }
        }

        [Fact]
        public void CRC32_sample_value_stream_too_short_buffered()
        {
            var buffer = Enumerable.Range(0, 10000000).Select(i => (byte)i).ToArray();
            using (var stream = new MemoryStream(buffer))
            {
                Assert.Throws<ArgumentException>(() => 
                    Checksum.CRC32(stream, 0, buffer.Length + 1).ToString("X8"));
            }
        }

        [Fact]
        public void CRC32_sample_value_step()
        {
            var buffer = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };

            var a = Checksum.Seed;
            var b = Checksum.UpdateCRC32(buffer.AsSpan(0, 4), a);
            var c = Checksum.UpdateCRC32(buffer.AsSpan(4), b);

            var crc = Checksum.FinalizeCRC32(c);

            // See http://www.sunshine2k.de/coding/javascript/crc/crc_js.html
            // See http://www.lammertbies.nl/comm/info/crc-calculation.html
            Assert.Equal("CBF43926", crc.ToString("X8"));
        }
    }
}
