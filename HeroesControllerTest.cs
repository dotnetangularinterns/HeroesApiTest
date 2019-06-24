using HeroesApi;
using HeroesApi.Controllers;
using HeroesApi.interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace HeroesApiTest
{
    public class HeroesControllerTest
    {

        HeroesController controller;
        IHeroesService service;

        public HeroesControllerTest() 
        {
            service = new HeroesServiceFake();
            controller = new HeroesController(service);
        }

        [Fact]
        public void Get_ShouldReturnOkResult_WhenCalled()
        {
            ActionResult<IEnumerable<Hero>> res = controller.Get(null);

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void Get_ShouldReturnAllItems_WhenCalled()
        {
            //Act
            OkObjectResult res = controller.Get(null).Result as OkObjectResult;

            //Assert
            List<Hero> heroes = Assert.IsType<List<Hero>>(res.Value);
            Assert.Equal(4, heroes.Count);
        }

        [Fact]
        public void GetById_ShouldReturnOkResult_WhenCalled()
        {
            ActionResult<Hero> res = controller.Get(1);

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetById_ShouldReturnNotFound_WhenUnknownIdPassed()
        {
            ActionResult<Hero> res = controller.Get(10);

            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public void GetById_ShouldReturnOkResult_WhenExisitingIdPassed()
        {
            ActionResult<Hero> res = controller.Get(1);

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectHero_WhenExisitingIdPassed()
        {
            OkObjectResult res = controller.Get(1).Result as OkObjectResult;

            Assert.IsType<Hero>(res.Value);
            Assert.Equal(1, (res.Value as Hero).Id);
        }

        [Fact]
        public void Get_ShouldReturnCorrectHeroes_WhenExistingNameIsPassed()
        {
            OkObjectResult res = controller.Get("z").Result as OkObjectResult;

            List<Hero> heroes = Assert.IsType<List<Hero>>(res.Value);
            Assert.Single(heroes);
        }

        [Fact]
        public void Get_ShouldReturnNoHeroes_WhenUnknownNameIsPassed()
        {
            OkObjectResult res = controller.Get("hehe").Result as OkObjectResult;

            List<Hero> heroes = Assert.IsType<List<Hero>>(res.Value);
            Assert.Empty(heroes);
        }

        [Fact]
        public void Post_ShouldReturnCreatedResponse_WhenValidModelPasses()
        {
            Hero hero = new Hero()
            {
                Name = "Jojo",
                Pic = "",
                Power = 15
            };

            ActionResult res = controller.Post(hero);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Post_ShouldReturnResponseCreateHero_WhenValidModelPasses()
        {
            Hero hero = new Hero()
            {
                Name = "Jojo",
                Pic = "",
                Power = 15
            };

            CreatedAtActionResult res = controller.Post(hero) as CreatedAtActionResult;
            Hero mHero = res.Value as Hero;

            Assert.IsType<Hero>(mHero);
            Assert.Equal("Jojo", mHero.Name);
        }

        [Fact]
        public void Post_ShouldReturnBadRequest_WhenInvalidModelPasses()
        {
            Hero hero = new Hero()
            {
                Pic = "",
                Power = 15
            };

            controller.ModelState.AddModelError("Name", "Required");

            ActionResult res = controller.Post(hero);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void Put_ShouldReturnAcceptedResponse_WhenValidModelPassed()
        {
            Hero hero = new Hero()
            {
                Id = 1,
                Name = "Pasha",
                Pic = "https://spng.pngfly.com/20190305/zga/kisspng-t-shirt-dabbing-unicorn-mens-tank-top-dabbing-un-unicorn-dab-sticker-by-ona-maria-acuna-escano-5c7ee36e662318.0101235515518196304184.jpg",
                Power = 5
            };

            ActionResult res = controller.Put(hero);

            Assert.IsType<AcceptedAtActionResult>(res);
        }

        [Fact]
        public void Put_ShouldReturnResponseUpdateHero_WhenValidModelPassed()
        {
            Hero hero = new Hero()
            {
                Id = 1,
                Name = "Pasha",
                Pic = "https://spng.pngfly.com/20190305/zga/kisspng-t-shirt-dabbing-unicorn-mens-tank-top-dabbing-un-unicorn-dab-sticker-by-ona-maria-acuna-escano-5c7ee36e662318.0101235515518196304184.jpg",
                Power = 5
            };

            AcceptedAtActionResult res = controller.Put(hero) as AcceptedAtActionResult;
            Hero mHero = res.Value as Hero;

            Assert.IsType<Hero>(mHero);
            Assert.Equal(5, mHero.Power);
        }

        [Fact]
        public void Put_ShouldReturnBadRequest_WhenInvalidModelPassed()
        {
            Hero hero = new Hero()
            {
                Name = "Pasha",
                Pic = "",
                Power = 15
            };

            controller.ModelState.AddModelError("Id", "Required");

            ActionResult res = controller.Put(hero);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void Put_ShouldReturnNotFound_WhenUnknownIdPassed()
        {

            Hero hero = new Hero()
            {
                Id = 10,
                Name = "Pasha",
                Pic = "https://spng.pngfly.com/20190305/zga/kisspng-t-shirt-dabbing-unicorn-mens-tank-top-dabbing-un-unicorn-dab-sticker-by-ona-maria-acuna-escano-5c7ee36e662318.0101235515518196304184.jpg",
                Power = 5
            };

            ActionResult<Hero> res = controller.Put(hero);

            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenUnknownIdPassed()
        {
            int id = 20;

            ActionResult res = controller.Delete(id);

            Assert.IsType<NotFoundResult>(res);
         }

        [Fact]
        public void Delete_ShouldReturnOkResult_WhenExisitingIdPassed()
        {
            int id = 4;

            ActionResult res = controller.Delete(id);

            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public void Delete_ShouldRemoveSpecifiedHero_WhenExistingIdPassed()
        {
            int id = 4;

            ActionResult<Hero> res = controller.Delete(id);

            List<Hero> heroes = service.GetHeroes() as List<Hero>;

            Assert.Equal(3, heroes.Count);
        }

    }
}
