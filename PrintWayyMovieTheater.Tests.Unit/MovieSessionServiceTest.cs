﻿using Microsoft.Extensions.DependencyInjection;
using PrintWayyMovieTheater.Domain;
using PrintWayyMovieTheater.Domain.Entities;
using PrintWayyMovieTheater.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PrintWayyMovieTheater.Tests.Unit
{
    public class MovieSessionServiceTest
    {
        readonly IMovieSessionService _movieSessionService;
        readonly IMovieService _movieService;
        public MovieSessionServiceTest()
        {
            var serviceProvider = new ServiceCollection()
                                           .AddAllServices()
                                           .BuildServiceProvider();
            _movieSessionService = serviceProvider.GetService<IMovieSessionService>();
            _movieService = serviceProvider.GetService<IMovieService>();
        }

        [Fact]
        public void Create_ShouldExpectedChanges()
        {
            //Given
            var movieId = CreateMovie(1);
            var movieSession = new MovieSession
            {
                MovieId = movieId,
                MotionGraphics = MotionGraphics.ThreeDimensions,
                PresentationStart = DateTime.Now.AddDays(20),
                Room = new MovieRoom
                {
                    Name = "Room 1",
                    Seats = 50
                }
            };
            var expectedChanges = 2;

            //When
            var changes = _movieSessionService.Create(movieSession);

            //Then
            Assert.Equal(expectedChanges, changes);
        }

        [Fact]
        public void Delete_ShouldExpectedChanges()
        {
            //Given
            var movieId = CreateMovie(2);
            var movieSession = new MovieSession
            {
                MovieId = movieId,
                MotionGraphics = MotionGraphics.ThreeDimensions,
                PresentationStart = DateTime.Now.AddDays(10),
                Room = new MovieRoom
                {
                    Name = "Room 1",
                    Seats = 50
                }
            };
            _movieSessionService.Create(movieSession);
            var expectedChanges = 1;

            //When
            var changes = _movieSessionService.Delete(movieSession.Id);

            //Then
            Assert.Equal(expectedChanges, changes);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenPresentationStartLessThan10()
        {
            //Given
            var movieId = CreateMovie(3);
            var movieSession = new MovieSession
            {
                MovieId = movieId,
                MotionGraphics = MotionGraphics.ThreeDimensions,
                PresentationStart = DateTime.Now.AddDays(9),
                Room = new MovieRoom
                {
                    Name = "Room 1",
                    Seats = 50
                }
            };
            _movieSessionService.Create(movieSession);

            //When
            void action() => _movieService.Delete(movieSession.Id);

            //Then
            Assert.Throws<ValidationException>(action);
        }

        private int CreateMovie(int part)
        {
            var movie = new Movie
            {
                Title = $"Back to the Future - Part {part}",
                Duration = 116,
                Description = "Marty McFly, a 17-year-old high school student, is accidentally sent thirty years into the past in a time-traveling DeLorean invented by his close friend, the eccentric scientist Doc Brown."
            };
            _movieService.Create(movie);
            return movie.Id;
        }
    }
}
