IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetProductTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetProductTypes]
GO

	-- =============================================
-- Author	:	Sachin Bharti(27th Oct)
-- Description	:	Get all dealer renewal product types
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetProductTypes]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT 
			DAD.Id,
			DAD.Name
	FROM DCRM_ADM_DealerTypes DAD(NOLOCK)
END




/****** Object:  StoredProcedure [dbo].[DCRM_UpdateSalesStatus]    Script Date: 10/27/2014 4:59:12 PM ******/
SET ANSI_NULLS ON
