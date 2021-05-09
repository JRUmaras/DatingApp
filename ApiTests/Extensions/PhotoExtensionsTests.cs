using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using API.Entities;
using API.Extensions;
using NUnit.Framework;

namespace ApiTests.Extensions
{
    public class PhotoExtensionsTests
    {
        [SetUp]
        public void Setup()
        { }

        [Test]
        public void SetNewMainPhoto_OldMainAndNewMainExists_ReturnsTrue()
        {
            // Arrange
            var photos = new List<Photo>
            {
                new() {Id = 1, IsMain = true},
                new () {Id = 2},
                new () {Id = 3}
            };
            const int newMainPhotoId = 2;
            var photosEnumerated = photos.ToList();

            // Act
            var wasSuccess = photosEnumerated.SetNewMainPhoto(newMainPhotoId);

            // Assert
            Assert.IsTrue(wasSuccess);
            // Only single main exists.
            Assert.AreEqual(1, photosEnumerated.Count(photo => photo.IsMain));
            // The photo with the right ID was set as the new main
            Assert.AreEqual(newMainPhotoId, photosEnumerated.First(photo => photo.IsMain).Id);
        }

        [Test]
        public void SetNewMainPhoto_OldMainDoesNotExistAndNewMainExists_ReturnsTrue()
        {
            // Arrange
            var photos = new List<Photo>
            {
                new() { Id = 1 },
                new () { Id = 2 },
                new () { Id = 3 }
            };
            const int newMainPhotoId = 2;
            var photosEnumerated = photos.ToList();

            // Act
            var wasSuccess = photosEnumerated.SetNewMainPhoto(newMainPhotoId);

            // Assert
            Assert.IsTrue(wasSuccess);
            // Only single main exists.
            Assert.AreEqual(1, photosEnumerated.Count(photo => photo.IsMain));
            // The photo with the right ID was set as the new main
            Assert.AreEqual(newMainPhotoId, photosEnumerated.First(photo => photo.IsMain).Id);
        }

        [Test]
        public void SetNewMainPhoto_OldMainAndNewMainDoNotExist_ReturnsFalse()
        {
            // Arrange
            var photos = new List<Photo>
            {
                new() { Id = 1 },
                new () { Id = 2 },
                new () { Id = 3 }
            };
            const int newMainPhotoId = 4;
            var photosEnumerated = photos.ToList();

            // Act
            var wasSuccess = photosEnumerated.SetNewMainPhoto(newMainPhotoId);

            // Assert
            Assert.IsFalse(wasSuccess);
            // None of the photos is main
            Assert.AreEqual(0, photosEnumerated.Count(photo => photo.IsMain));
        }
    }
}
