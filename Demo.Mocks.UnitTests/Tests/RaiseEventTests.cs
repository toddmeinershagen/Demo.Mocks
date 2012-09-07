using System;
using System.Collections.Generic;
using Demo.Mocks.UnitTests.Source;
using NSubstitute;
using NUnit.Framework;

namespace Demo.Mocks.UnitTests.Tests
{
    [TestFixture]
    public class RaiseEventTests
    {
        [Test]
        public void given_presenter_when_view_loads_should_load_patients()
        {
            var view = Substitute.For<IView>();
            var presenter = new Presenter(view);

            view.Load += Raise.EventWith(EventArgs.Empty);

            Assert.That(presenter.Patients, Is.Not.Empty);
        }

        [Test]
        public void given_presenter_when_view_has_not_loaded_should_not_have_patients()
        {
            var view = Substitute.For<IView>();
            var presenter = new Presenter(view);

            Assert.That(presenter.Patients, Is.Null);
        }
    }

    public interface IView
    {
        event EventHandler Load;
    }

    class Presenter
    {
        private readonly IView _view;

        public Presenter(IView view)
        {
            _view = view;
            view.Load += view_Load;
        }

        void view_Load(object sender, EventArgs e)
        {
            Patients = new List<Patient> {new Patient(), new Patient()};
        }

        public List<Patient> Patients { get; set; }
    }
}
