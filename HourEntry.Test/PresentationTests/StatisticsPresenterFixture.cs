using System;
using System.Data;
using System.Web.UI.WebControls;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using _ = NMock2;

using Bll.HourEntry;
using PresentationLayer.HourEntry;
using UnitTests.HourEntry.Helpers;
using System.Collections.Generic;

namespace UnitTests.Presentation.HourEntry.Hours
{
    [TestFixture]
    public class StatisticsPresenterFixture
    {
        private bool _IsPostBack;

        private TextBox _txtFromDate;
        private TextBox _txtToDate;
        private RadioButtonList _radDateRangePicker;
        private DropDownList _ddlProjects;
        private TextBox _txtComments;

        private GridView _gvStatistics;

        [SetUp]
        protected virtual void SetUp()
        {
            this._IsPostBack = false;

            this._txtFromDate = new TextBox();
            this._txtToDate = new TextBox();
            this._radDateRangePicker = new RadioButtonList();
            this._ddlProjects = new DropDownList();
            this._txtComments = new TextBox();

            this._gvStatistics = new GridView();
        }

        [Test]
        public void PageLoad_Default()
        {
            _.Mockery mockery = new _.Mockery();
            IStatisticsView mockView = this.GetMockView_PageLoad(mockery);
            IProject mockProject = this.GetMockProject(mockery);

            StatisticsPresenter p = 
                new StatisticsPresenter(mockView, mockProject, new StatisticAnalyzer(), new HourStub());
            p.PageLoad(null, null);

            mockery.VerifyAllExpectationsHaveBeenMet();

            this.VerifyState_PageLoad();
        }

        private IStatisticsView GetMockView_PageLoad(_.Mockery mockery)
        {
            IStatisticsView mockView = (IStatisticsView)mockery.NewMock(typeof(IStatisticsView));

            _.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(_.Return.Value(this._IsPostBack));

            _.Expect.Once.On(mockView).GetProperty("FromDate").Will(_.Return.Value(this._txtFromDate));
            _.Expect.Once.On(mockView).GetProperty("ToDate").Will(_.Return.Value(this._txtToDate));
            _.Expect.Once.On(mockView).GetProperty("DateRangePicker").Will(_.Return.Value(this._radDateRangePicker));
            _.Expect.Once.On(mockView).GetProperty("Projects").Will(_.Return.Value(this._ddlProjects));

            return mockView;
        }

        private IProject GetMockProject(_.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));

            _.Expect.Once.On(mockProjects).Method("List").Will(_.Return.Value(helper.GetMockProjectData(true)));

            return mockProjects;
        }

        private void VerifyState_PageLoad()
        {
            if (this._IsPostBack)
                Assert.That(this._gvStatistics.Rows.Count, Is.GreaterThan(0), "No Rows");
            else
            {
                Assert.That(this._txtFromDate.Text, Is.EqualTo(DateTime.Today.ToShortDateString()),
                    "Wrong Default for From Date");
                Assert.That(this._txtToDate.Text, Is.EqualTo(DateTime.Today.ToShortDateString()),
                    "Wrong Default for To Date");

                Assert.That(this._radDateRangePicker.Items.Count, Is.EqualTo(5), "Wrong Count for Date Range Picker");
                Assert.That(this._radDateRangePicker.SelectedValue, Is.EqualTo(StatisticsPresenter.DateRangeEnum.Today.ToString("d")),
                    "Wrong Date Range Picker Default");

                Assert.That(this._ddlProjects.Items.Count, Is.GreaterThan(0), "No data in Project List");
                Assert.That(this._ddlProjects.Items.Count, Is.EqualTo(3), "Project List: wrong row count");
                Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo("0"),
                    "Wrong Selected Project ID");
            }
        }

        [Test]
        public void ButtonClick()
        {
            this._IsPostBack = true;

            _.Mockery mockery = new _.Mockery();
            IStatisticsView mockView = this.GetMockView_ShowReports_Click(mockery);
            IProject mockProject = this.GetMockProject_ShowReports_Click(mockery);
            IStatisticAnalyzer mockStats = this.GetMockStatistics_ShowReports_Click(mockery);

            StatisticsPresenter p = new StatisticsPresenter(mockView, mockProject, mockStats, new HourStub());
            p.ShowReports_Click(null, null);

            mockery.VerifyAllExpectationsHaveBeenMet();

            this.VerifyState_PageLoad();
        }

        private IStatisticsView GetMockView_ShowReports_Click(_.Mockery mockery)
        {
            IStatisticsView mockView = (IStatisticsView)mockery.NewMock(typeof(IStatisticsView));

            _.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(_.Return.Value(this._IsPostBack));

            _.Expect.Once.On(mockView).GetProperty("FromDate").Will(_.Return.Value(this._txtFromDate));
            _.Expect.Once.On(mockView).GetProperty("ToDate").Will(_.Return.Value(this._txtToDate));
            _.Expect.Once.On(mockView).GetProperty("Projects").Will(_.Return.Value(this._ddlProjects));
            _.Expect.Once.On(mockView).GetProperty("Comments").Will(_.Return.Value(this._txtComments));

            _.Expect.Once.On(mockView).GetProperty("Statistics").Will(_.Return.Value(this._gvStatistics));

            return mockView;
        }

        private IProject GetMockProject_ShowReports_Click(_.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));

            return mockProjects;
        }

        private IStatisticAnalyzer GetMockStatistics_ShowReports_Click(_.Mockery mockery)
        {
            IStatisticAnalyzer mockInfo = (IStatisticAnalyzer)mockery.NewMock(typeof(IStatisticAnalyzer));

            _.Expect.Once.On(mockInfo).SetProperty("Hours");
            _.Expect.Once.On(mockInfo).SetProperty("Projects");
            _.Expect.Once.On(mockInfo).Method("sumhours")
                .With(DateTime.Today, DateTime.Today, 0, "")
                .Will(_.Return.Value(this.GetMockData()));

            return mockInfo;
        }
        private List<StatisticEntry> GetMockData()
        {
            List<StatisticEntry> hours = new List<StatisticEntry>();
            StatisticEntry dr = new StatisticEntry("", 2.5M, "", DateTime.Today);
            hours.Add(dr);

            return hours;
        }

        private class HourStub : IHour
        {
            string IHour.DataPath
            {
                set { throw new NotImplementedException(); }
            }

            DataTable IHour.List()
            {
                throw new NotImplementedException();
            }

            DataTable IHour.GetRecord(int id)
            {
                throw new NotImplementedException();
            }

            void IHour.Update(int id, int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments)
            {
                throw new NotImplementedException();
            }

            int IHour.Add(int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments)
            {
                throw new NotImplementedException();
            }
        }

    }
}
