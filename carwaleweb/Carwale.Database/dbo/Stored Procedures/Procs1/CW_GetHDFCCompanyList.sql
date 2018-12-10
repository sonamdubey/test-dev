IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetHDFCCompanyList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetHDFCCompanyList]
GO

	-- =============================================
-- Author:		Mihir Chheda
-- Create date: 04-08-2015
-- Description:	Bind list of company provided by HDFC 
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetHDFCCompanyList]
AS
BEGIN
   SELECT Id AS Value,CompanyName AS Text 
   FROM   CW_CompanyList WITH (NOLOCK)
   WHERE  IsActive=1 
   ORDER BY  CompanyName 
END
