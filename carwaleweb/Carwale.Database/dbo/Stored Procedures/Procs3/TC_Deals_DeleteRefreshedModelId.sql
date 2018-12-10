IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_DeleteRefreshedModelId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_DeleteRefreshedModelId]
GO

	
-- =============================================
-- Author		: Nilima More
-- Created Date : 18th Jan 2016.
-- Description  : To get model Status.
-- EXEC [TC_Deals_DeleteRefreshedModelId] 
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_DeleteRefreshedModelId] 
@modelId [TC_Deals_ModelId] READONLY

AS
BEGIN
	 --Delete all rows after Refreshing MemCache.
    DELETE  FROM DCRM_Deals_ModelStatus WHERE TC_ModelId IN (SELECT TC_ModelId FROM @modelId)

END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
