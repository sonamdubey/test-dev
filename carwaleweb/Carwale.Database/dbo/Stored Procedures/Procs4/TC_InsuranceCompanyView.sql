IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsuranceCompanyView]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsuranceCompanyView]
GO

	
-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 5th October 2011
-- Description:	This procedure will be used to View All Dealers Bank for Finance
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsuranceCompanyView]
(
@DealerId NUMERIC
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT TC_InsuranceCompany_Id ,CompanyName FROM TC_InsuranceCompany 
	WHERE DealerId=@DealerId AND IsActive=1	    
END


