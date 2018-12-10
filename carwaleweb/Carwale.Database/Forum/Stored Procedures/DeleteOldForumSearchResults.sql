IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[Forum].[DeleteOldForumSearchResults]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [Forum].[DeleteOldForumSearchResults]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 07-26-2011
-- Description:	Delete records from ForumSearchResults which are 1 hour old
-- =============================================
CREATE PROCEDURE Forum.DeleteOldForumSearchResults
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE ForumSearchResults
	FROM ForumSearchResults AS fsr WITH (NOLOCK)
	   JOIN forumsearches AS fs WITH (NOLOCK) ON fsr.SearchId=fs.Id
	WHERE fs.SearchDateTime < DATEADD(hh,-1,GETDATE())
END
