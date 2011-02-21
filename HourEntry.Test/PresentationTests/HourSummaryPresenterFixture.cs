using System;
using System.Data;
using System.Web.UI.WebControls;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

using Bll.HourEntry;
using PresentationLayer.HourEntry;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.Presentation.HourEntry.Hours
{
    [TestFixture]
    public class HourSummaryPresenterFixture
    {
        private GridView _gvHoursPerMonthList;
        private GridView _gvHoursPerMonthPerProjectList;
        private GridView _gvHoursPerWeekPerProjectList;

        [SetUp]
        protected void SetUp()
        {
            this._gvHoursPerMonthList = new GridView();
            this._gvHoursPerMonthPerProjectList = new GridView();
            this._gvHoursPerWeekPerProjectList = new GridView();
        }

        [Test]
        public void PageLoad()
        {
            mock.Mockery mockery = new mock.Mockery();
            IHourSummaryView mockView = this.GetMockView(mockery);
            IHour mockHour = this.GetMockHour(mockery);
            IProject mockProject = this.GetMockProject(mockery);

            HourSummaryPresenter p = new HourSummaryPresenter(mockView, mockHour, mockProject);
            p.PageLoad(null, null);

            Assert.That(this._gvHoursPerMonthList.Rows.Count, Is.GreaterThan(0), "No data in Hours Per Month List");
            Assert.That(this._gvHoursPerMonthList.Rows.Count, Is.EqualTo(1), "Hours Per Month List: wrong row count");
            Assert.That(this._gvHoursPerMonthPerProjectList.Rows.Count, Is.GreaterThan(0), "No data in Hours Per Month Per Project List");
            Assert.That(this._gvHoursPerMonthPerProjectList.Rows.Count, Is.EqualTo(1), "Hours Per Month Per Project List: wrong row count");
            Assert.That(this._gvHoursPerWeekPerProjectList.Rows.Count, Is.GreaterThan(0), "No data in Hours Per Week Per Project List");
            Assert.That(this._gvHoursPerWeekPerProjectList.Rows.Count, Is.EqualTo(1), "Hours Per Month Per Week List: wrong row count");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IHourSummaryView GetMockView(mock.Mockery mockery)
        {
            IHourSummaryView mockView = (IHourSummaryView)mockery.NewMock(typeof(IHourSummaryView));
            mock.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(mock.Return.Value(false));
            this.LoadMockHourEntries(mockView);

            return mockView;
        }
        private void LoadMockHourEntries(IHourSummaryView mockView)
        {
            mock.Expect.Once.On(mockView).GetProperty("HoursPerMonthList").Will(mock.Return.Value(this._gvHoursPerMonthList));
            mock.Expect.Once.On(mockView).GetProperty("HoursPerMonthPerProjectList").Will(mock.Return.Value(this._gvHoursPerMonthPerProjectList));
            mock.Expect.Once.On(mockView).GetProperty("HoursPerWeekPerProjectList").Will(mock.Return.Value(this._gvHoursPerWeekPerProjectList));
        }
        private IHour GetMockHour(mock.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            this.ListHours(mockHours);

            return mockHours;
        }
        private void ListHours(IHour mockHours)
        {
            mock.Expect.Once.On(mockHours).Method("List").Will(mock.Return.Value(helper.GetMockHoursData()));
        }
        private IProject GetMockProject(mock.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            this.ListProjects(mockProjects);

            return mockProjects;
        }
        private void ListProjects(IProject mockProjects)
        {
            mock.Expect.Once.On(mockProjects).Method("List").Will(mock.Return.Value(helper.GetMockProjectData()));
        }
    }
}
