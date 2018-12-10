IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetAllProfileIdLIst]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetAllProfileIdLIst]
GO

	---------------------------------------------------
-- Created By : Sadhana Upadhyay on 16 June 2015
-- Summary : To get All Profile Ids to Bulk Update Elastic Index
---------------------------------------------------
CREATE PROCEDURE [dbo].[ES_GetAllProfileIdLIst]
AS
BEGIN
	SELECT ProfileId
	FROM livelistings WITH (NOLOCK)
END