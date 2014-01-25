/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Types.Interfaces
{
	#region Using

    using System;
    using System.Collections.Generic;

    #endregion

	/// <summary>
	/// The read track current user extensions.
	/// </summary>
	public static class IReadTrackCurrentUserExtensions
	{
		#region Public Methods

		/// <summary>
		/// The get forum topic read.
		/// </summary>
		/// <param name="readTrackCurrentUser">
		/// The read track current user.
		/// </param>
		/// <param name="forumId">
		/// The forum id.
		/// </param>
		/// <param name="topicId">
		/// The topic id.
		/// </param>
		/// <param name="forumReadOverride">
		/// The forum read override.
		/// </param>
		/// <param name="topicReadOverride">
		/// The topic read override.
		/// </param>
		/// <returns>
		/// </returns>
		public static DateTime GetForumTopicRead(
			this IReadTrackCurrentUser readTrackCurrentUser, 
			int forumId, 
			int topicId, 
			DateTime? forumReadOverride = null, 
			DateTime? topicReadOverride = null)
		{
			CodeContracts.VerifyNotNull(readTrackCurrentUser, "readTrackCurrentUser");

			DateTime lastRead = readTrackCurrentUser.GetTopicRead(topicId, topicReadOverride);
			DateTime lastReadForum = readTrackCurrentUser.GetForumRead(forumId, forumReadOverride);

			if (lastReadForum > lastRead)
			{
				lastRead = lastReadForum;
			}

			return lastRead;
		}

		/// <summary>
		/// The set forum read.
		/// </summary>
		/// <param name="readTrackCurrentUser">
		/// The read track current user. 
		/// </param>
		/// <param name="forumIds">
		/// The forum ids. 
		/// </param>
		public static void SetForumRead(this IReadTrackCurrentUser readTrackCurrentUser, IEnumerable<int> forumIds)
		{
			CodeContracts.VerifyNotNull(readTrackCurrentUser, "readTrackCurrentUser");
			CodeContracts.VerifyNotNull(forumIds, "forumIds");

			foreach (var id in forumIds)
			{
				readTrackCurrentUser.SetForumRead(id);
			}
		}

		/// <summary>
		/// The set topic read.
		/// </summary>
		/// <param name="readTrackCurrentUser">
		/// The read track current user. 
		/// </param>
		/// <param name="topicIds">
		/// The topic ids. 
		/// </param>
		public static void SetTopicRead(this IReadTrackCurrentUser readTrackCurrentUser, IEnumerable<int> topicIds)
		{
			CodeContracts.VerifyNotNull(readTrackCurrentUser, "readTrackCurrentUser");
			CodeContracts.VerifyNotNull(topicIds, "topicIds");

			foreach (var id in topicIds)
			{
				readTrackCurrentUser.SetTopicRead(id);
			}
		}

		#endregion
	}
}