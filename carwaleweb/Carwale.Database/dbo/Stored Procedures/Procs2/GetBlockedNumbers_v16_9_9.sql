IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBlockedNumbers_v16_9_9]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBlockedNumbers_v16_9_9]
GO

	
-- =============================================
-- Author:		Vinayak					
-- Create date: 5/8/2016
-- Description:	get blocked numbers
-- Modified : Vicky Lund, 27/09/2016, Fetched Id column and renamed column name
-- Modified : Vicky Lund, 28/10/2016, Fetched IsAutoInserted column (done)
-- =============================================
CREATE PROCEDURE [dbo].[GetBlockedNumbers_v16_9_9]
AS
BEGIN
	SELECT BN.Id
		,BN.Mobile [Name]
		,BN.IsAutoInserted
	FROM BlockedNumbers BN WITH (NOLOCK)
END
