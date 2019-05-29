using System;
using Xunit;

namespace denemesig.Tests
{
    public class CloudStorageUnitTest
    {
        [Fact]
        public void ValidateConnectionId()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Throws<ArgumentException>( () =>  cloudStorage.InsertMessage(null, "user", "message"));
        }
        [Fact]
        public void ValidateUser()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Throws<ArgumentException>( () =>  cloudStorage.InsertMessage("connectionId", null, "message"));
        }
        [Fact]
        public void ValidateMessage()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Throws<ArgumentException>( () =>  cloudStorage.InsertMessage("connectionId", "user", null));
        }
        [Fact]
        public void ValidateInsertMessageConnectionId()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Equal("connectionId", cloudStorage.InsertMessage("connectionId", "user", "message").ConnectionID);
        }
        [Fact]
        public void ValidateInsertMessageUser()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Equal("user", cloudStorage.InsertMessage("connectionId", "user", "message").Username);
        }
        [Fact]
        public void ValidateInsertMessageMessage()
        {
            CloudStorage cloudStorage = new CloudStorage();
            Assert.Equal("message", cloudStorage.InsertMessage("connectionId", "user", "message").Message);
        }
    }
}