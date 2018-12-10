IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_SaveAPICallLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_SaveAPICallLog]
GO

	-- =============================================================================
-- Author		: Chetan Navin
-- Created Date : 15th Dec, 2015
-- Description	: To save api call log
-- =============================================================================
CREATE PROCEDURE [dbo].[TC_CarTrade_SaveAPICallLog] @RequestURL VARCHAR(2000)
	,@RequestBody VARCHAR(2000)
	,@Response VARCHAR(2000) = NULL
	,@Status SMALLINT = NULL 
	,@ProductId TINYINT
	,@ProductItemId INT
AS
BEGIN
	INSERT INTO TC_CarTradeAPILog (
		RequestURL
		,RequestDate
		,RequestBody
		,Response
		,STATUS
		,ProductId
		,ProductItemId
		)
	VALUES (
		@RequestURL
		,GETDATE()
		,@RequestBody
		,@Response
		,@Status
		,@ProductId
		,@ProductItemId
		)
END


