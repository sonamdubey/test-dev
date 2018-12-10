IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerHasCertification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerHasCertification]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24th April,2013
-- Description:	Checks Whether the Dealer Has CertificationId or not
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerHasCertification]

@DealerId INT

AS
BEGIN
	SELECT CertificationId FROM Dealers WITH(NOLOCK) WHERE ID=@DealerId
	AND (CertificationId <> -1 OR CertificationId <> NULL)
END

