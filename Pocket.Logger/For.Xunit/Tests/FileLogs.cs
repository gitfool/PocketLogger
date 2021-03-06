﻿using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using static Pocket.Logger<Pocket.For.Xunit.Tests.FileLogs>;

namespace Pocket.For.Xunit.Tests
{
    public class FileLogs
    {
        [Fact]
        public void When_writeToFile_is_set_to_false_then_no_file_is_written()
        {
            var attribute = new LogToPocketLoggerAttribute(writeToFile: false);

            var methodInfo = GetType().GetMethod(nameof(When_writeToFile_is_set_to_false_then_no_file_is_written));

            attribute.Before(methodInfo);

            Log.Info("hello from {method}", methodInfo.Name);

            var file = TestLog.Current.LogFile;

            attribute.After(methodInfo);

            file.Should().BeNull();
        }

        [Fact]
        public void When_writeToFile_is_set_to_true_then_a_file_is_written()
        {
            var attribute = new LogToPocketLoggerAttribute(writeToFile: true);

            var methodInfo = GetType().GetMethod(nameof(When_writeToFile_is_set_to_true_then_a_file_is_written));

            attribute.Before(methodInfo);

            var file = TestLog.Current.LogFile;
            var message = "hello from " + methodInfo.Name + $" ({Guid.NewGuid()})";

            Log.Info(message);

            attribute.After(methodInfo);

            file.Should().NotBeNull();
            file.Exists.Should().BeTrue();

            var text = File.ReadAllText(file.FullName);

            text.Should().Contain(message);
        }
      
        [Fact]
        public void When_filename_is_set_then_log_output_is_written_to_the_specified_file()
        {
            var filename = $"{Guid.NewGuid()}.log";
            var attribute = new LogToPocketLoggerAttribute(filename: filename);

            var methodInfo = GetType().GetMethod(nameof(When_filename_is_set_then_log_output_is_written_to_the_specified_file));

            attribute.Before(methodInfo);

            var file = TestLog.Current.LogFile;
            var message = "hello from " + methodInfo.Name + $" ({Guid.NewGuid()})";

            Log.Info(message);

            attribute.After(methodInfo);

            file.Should().NotBeNull();
            file.Exists.Should().BeTrue();

            var text = File.ReadAllText(file.FullName);

            text.Should().Contain(message);

            file.Name.Should().Be(filename);
        }
    }
}
