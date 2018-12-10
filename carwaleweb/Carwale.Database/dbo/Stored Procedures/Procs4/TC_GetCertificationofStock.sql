IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCertificationofStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCertificationofStock]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 4th Nov 2014
-- Description:	To check if the stock is certified or uncertified
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCertificationofStock]
	@StockId INT,
	@IsCertified BIT OUTPUT
AS
BEGIN
	DECLARE @CertId INT
	SELECT @CertId = CertificationId FROM TC_Stock WHERE ID = @StockId
	IF @CertId IS NOT NULL AND @CertId <> -1
		SET	@IsCertified = 1
	ELSE
		SET	@IsCertified = 0
END

