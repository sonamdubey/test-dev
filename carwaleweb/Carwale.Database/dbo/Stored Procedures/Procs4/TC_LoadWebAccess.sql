IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoadWebAccess]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoadWebAccess]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 7th March,2013
-- Description:	TC_LoadWebAccess 46
-- =============================================
CREATE PROCEDURE [dbo].[TC_LoadWebAccess] 
@DealerId Bigint
AS
BEGIN
	SELECT HavingWebsite,PaidDealer FROM Dealers WITH(NOLOCK) WHERE Id=@DealerId
END