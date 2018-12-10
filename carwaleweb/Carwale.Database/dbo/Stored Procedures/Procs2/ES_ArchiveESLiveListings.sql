IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_ArchiveESLiveListings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_ArchiveESLiveListings]
GO

	-- =======================================================================================
-- Author:		Manish Chourasiya
-- Create date: 03 July 2015
-- Description:	SP for archiving data of ES_LiveListings table
-- Modified by Manish on 18-10-2016 commented the code for inserting into the table ES_LiveListingsArchive since this is for 
-- debugging purpose of elastic search sync in case of any issue. Now process streamlined we can stop logging.
-- ========================================================================================
CREATE PROCEDURE [dbo].[ES_ArchiveESLiveListings]
 AS 
   BEGIN

		/*CREATE TABLE #TempES
		   (
			[ProfileID] [varchar](50),
			[Action] [char](6),
			[ActionType] [tinyint] ,
			[LastUpdateTime] [datetime] ,
			[IsSynced] [bit] ,
			[SyncTime] [datetime] 
		   )*/

			DELETE 
			FROM ES_LiveListings
			--OUTPUT deleted.* INTO #TempES
			WHERE IsSynced=1;

			/*INSERT INTO  ES_LiveListingsArchive
												(ProfileID,
												 [Action],
												 ActionType,
												 LastUpdateTime,
												 IsSynced,
												 SyncTime
												 )
                                   SELECT        ProfileID,
												 [Action],
												 ActionType,
												 LastUpdateTime,
												 IsSynced,
												 SyncTime
                                    FROM #TempES;*/
    END

