/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bj�rnar Henden
 * Copyright (C) 2006-2012 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YAF.Pages.Admin
{
    #region Using

    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    using YAF.Classes;
    using YAF.Classes.Data;
    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Interfaces;
    using YAF.Utils;
    using YAF.Utils.Helpers;

    #endregion

    /// <summary>
    /// The Admin Event Log Page.
    /// </summary>
    public partial class eventlog : AdminPage
    {
        #region Methods

        /// <summary>
        /// Delete Selected Event Log Entry
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void DeleteAll_Click([NotNull] object sender, [NotNull] EventArgs e)
        {
            LegacyDb.eventlog_deletebyuser(this.PageContext.PageBoardID, this.PageContext.PageUserID);

            // re-bind controls
            this.BindData();
        }

        /// <summary>
        /// Handles load event for delete all button.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <remarks>
        /// Adds confirmation popup to click event of this button.
        /// </remarks>
        protected void DeleteAll_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            ((Button)sender).Text = this.GetText("ADMIN_EVENTLOG", "DELETE_ALLOWED");
            ControlHelper.AddOnClickConfirmDialog(sender, this.GetText("ADMIN_EVENTLOG", "CONFIRM_DELETE_ALL"));
        }

        /// <summary>
        /// Handles load event for log entry delete link button.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <remarks>
        /// Adds confirmation popup to click event of this button.
        /// </remarks>
        protected void Delete_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            ControlHelper.AddOnClickConfirmDialog(sender, this.GetText("ADMIN_EVENTLOG", "CONFIRM_DELETE"));
        }

        /// <summary>
        /// Gets HTML IMG code representing given log event icon.
        /// </summary>
        /// <param name="dataRow">
        /// Data row containing event log entry data.
        /// </param>
        /// <returns>
        /// return HTML code of event log entry image
        /// </returns>
        protected string EventImageCode([NotNull] object dataRow)
        {
            // cast object to the DataRowView
            var row = (DataRowView)dataRow;

            // set defaults
            string imageType = Enum.GetName(typeof (EventLogTypes), (int) row["Type"]);
            
            if (imageType.IsNotSet())
            {
                imageType = "Error";
            }
            // var imageFile = new FileInfo(YafForumInfo.GetURLToResource("icons/{0}.png".FormatWith(imageType.ToLowerInvariant())));
            // return HTML code of event log entry image
            return
                @"<img src=""{0}"" alt=""{1}"" title=""{1}"" />".FormatWith(
                    YafForumInfo.GetURLToResource("icons/{0}.png".FormatWith(imageType.ToLowerInvariant())), imageType);
        }

        /// <summary>
        /// Gets HTML IMG code representing given log event icon.
        /// </summary>
        /// <param name="dataRow">
        /// Data row containing event log entry data.
        /// </param>
        /// <returns>
        /// return HTML code of event log entry image
        /// </returns>
        protected string EventCssClass([NotNull] object dataRow)
        {
            // cast object to the DataRowView
            var row = (DataRowView)dataRow;

            string cssClass;

            // find out of what type event log entry is
            string imageType = Enum.GetName(typeof(EventLogTypes), (int)row["Type"]);
           // var imageFile = new FileInfo(YafForumInfo.GetURLToResource("icons/{0}.png".FormatWith(imageType.ToLowerInvariant())));
                
            if (imageType.IsNotSet())
            {
                imageType = "Error";
            }
            cssClass = imageType.IsSet() ? "ui-state-{0}".FormatWith(imageType.ToLowerInvariant()) : "ui-state-error";
            return cssClass;
        }

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit([NotNull] EventArgs e)
        {
            this.List.ItemCommand += this.List_ItemCommand;

            base.OnInit(e);
        }

        /// <summary>
        /// Page load event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            // do it only once, not on postbacks
            if (this.IsPostBack)
            {
                return;
            }

            // create page links
            // board index first
            this.PageLinks.AddLink(this.Get<YafBoardSettings>().Name, YafBuildLink.GetLink(ForumPages.forum));

            // administration index second
            this.PageLinks.AddLink(
                this.GetText("ADMIN_ADMIN", "Administration"), YafBuildLink.GetLink(ForumPages.admin_admin));

            this.PageLinks.AddLink(this.GetText("ADMIN_EVENTLOG", "TITLE"), string.Empty);

            this.Page.Header.Title = "{0} - {1}".FormatWith(
                this.GetText("ADMIN_ADMIN", "Administration"), this.GetText("ADMIN_EVENTLOG", "TITLE"));

            this.PagerTop.PageSize = 25;

            // bind data to controls
            this.BindData();
        }

        /// <summary>
        /// The pager top_ page change.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void PagerTop_PageChange([NotNull] object sender, [NotNull] EventArgs e)
        {
            // rebind
            this.BindData();
        }

        /// <summary>
        /// Populates data source and binds data to controls.
        /// </summary>
        private void BindData()
        {
            int baseSize = this.Get<YafBoardSettings>().MemberListPageSize;
            int nCurrentPageIndex = this.PagerTop.CurrentPageIndex;
            this.PagerTop.PageSize = baseSize;
           
            // list event for this board
            DataTable dt = LegacyDb.eventlog_list(
                this.PageContext.PageBoardID,
                this.PageContext.PageUserID,
                this.Get<YafBoardSettings>().EventLogMaxMessages,
                this.Get<YafBoardSettings>().EventLogMaxDays, 
                nCurrentPageIndex, 
                baseSize, 
                DateTimeHelper.SqlDbMinTime(), 
                DateTime.UtcNow, 
                null);

            this.List.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                this.PagerTop.Count = dt.AsEnumerable().First().Field<int>("TotalRows");
            }
            else
            {
                this.PagerTop.Count = 0;
            }

            // bind data to controls
            this.DataBind();

            
        }

        /// <summary>
        /// Handles single record commands in a repeater.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs"/> instance containing the event data.</param>
        private void List_ItemCommand([NotNull] object source, [NotNull] RepeaterCommandEventArgs e)
        {
            // what command are we serving?
            switch (e.CommandName)
            {
                    // delete log entry
                case "delete":

                    // delete just this particular log entry
                    LegacyDb.eventlog_delete(e.CommandArgument, this.PageContext.PageUserID);

                    // re-bind controls
                    this.BindData();
                    break;

                    // show/hide log entry details
                /*case "show":

                    // get details control
                    Control ctl = e.Item.FindControl("details");

                    // find link button control
                    var showbutton = e.Item.FindControl("showbutton") as LinkButton;

                    // invert visibility
                    ctl.Visible = !ctl.Visible;

                    // change visibility state of detail and label of linkbutton too
                    showbutton.Text = ctl.Visible
                                          ? this.GetText("ADMIN_EVENTLOG", "HIDE")
                                          : this.GetText("ADMIN_EVENTLOG", "SHOW");

                    break;*/
            }
        }

        #endregion
    }
}