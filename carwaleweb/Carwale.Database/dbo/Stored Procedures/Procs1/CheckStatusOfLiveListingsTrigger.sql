IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckStatusOfLiveListingsTrigger]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckStatusOfLiveListingsTrigger]
GO

	
-- =============================================
-- Author:		Manish Chourasiya
-- Create date: 23-6-2015
-- Description:	Identify the status of the trigger on Livelistings table whether it is enable or disable.
-- =============================================
CREATE PROCEDURE CheckStatusOfLiveListingsTrigger
AS
BEGIN
		SELECT  T.name TriggerName,
				CASE WHEN is_disabled=0 THEN 'Enable' ELSE 'Disable' END TriggerStatus
			   ,O.name TableName
		  FROM sys.triggers as t
		  JOIN SYS.objects AS O ON T.Parent_id=O.object_id
		  WHERE O.name='Livelistings'
END 