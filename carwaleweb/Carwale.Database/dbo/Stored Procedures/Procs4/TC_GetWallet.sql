IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetWallet]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetWallet]
GO

	

-- =============================================
-- Author	:	  Vinay Kumar Prajapati  6th  jan 2016
-- Purpose  :   To get All Ewallet 
-- =============================================

CREATE  PROCEDURE [dbo].[TC_GetWallet]
AS
BEGIN
	SET NOCOUNT ON;
	 SELECT EW.Name,EW.Id  FROM TC_EWallets AS EW WITH(NOLOCK) WHERE EW.IsActive=1	ORDER BY EW.Id
END

