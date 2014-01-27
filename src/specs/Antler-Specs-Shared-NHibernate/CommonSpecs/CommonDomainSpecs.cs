// ReSharper disable InconsistentNaming

using FluentAssertions;
using NHibernate;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.NHibernate.CommonSpecs
{
    public static class CommonDomainSpecs
    {
        public class when_trying_to_query_using_nhibernate_internal_session_directly
        {
            public static void should_return_result()
            {
                UnitOfWork.Do(uow =>
                    {
                        //arrange
                        var country1 = new Country {Name = "USA", Language = "English"};
                        uow.Repo<Country>().Insert(country1);

                        var country2 = new Country {Name = "Mexico", Language = "Spanish"};
                        uow.Repo<Country>().Insert(country2);

                        var team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg", Country = country1};
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() {Name = "Awesome", BusinessGroup = "AwesomeBg", Country = country2};
                        uow.Repo<Team>().Insert(team2);

                        //act                    
                        var internalSession = uow.CurrentSession.GetInternal<ISession>();
                        var result = internalSession.QueryOver<Team>().Where(t => t.Name == "Awesome").List();

                        //assert
                        result.Count.Should().Be(1);
                        result[0].Id.Should().Be(team2.Id);
                        result[0].Name.Should().Be("Awesome");
                        result[0].BusinessGroup.Should().Be("AwesomeBg");
                        result[0].Country.Name.Should().Be("Mexico");
                    });
            }
        }
    }
}
// ReSharper restore InconsistentNaming