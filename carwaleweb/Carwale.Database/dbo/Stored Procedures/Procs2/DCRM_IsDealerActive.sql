IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_IsDealerActive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_IsDealerActive]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(18th June 2014)
-- Description	:	Get status of the Dealer that is he active or inactive
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_IsDealerActive] 
	@DealerId	INT 
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT ISNULL(Status,0) AS Status FROM Dealers  WITH (NOLOCK) WHERE ID = @DealerId
END

