IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealerDetails]
GO

	-- =============================================
-- Author:		Nilima More
-- Create date: 30 th july
-- Description:	get dealer details.
-- execute [dbo].[AbSure_GetDealerDetails] 5
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetDealerDetails]  
	@DealerId INT
AS
BEGIN

	DECLARE @IsRCPending BIT
	
	IF EXISTS(SELECT Id FROM AbSure_CarDetails WHERE DealerId = @DealerId AND RCImagePending = 1)
		SET @IsRCPending = 1
	ELSE
		SET @IsRCPending = 0

	SELECT @IsRCPending IsRCPending
	--SELECT ISNULL(RCNotMandatory,0) RCNotMandatory
	--FROM Dealers WITH(NOLOCK)
	--WHERE ID = @DealerId

	--SELECT  DISTINCT ACD.Id AbSure_CardetailsId
	--FROM AbSure_CarDetails ACD WITH(NOLOCK)
	--INNER JOIN AbSure_CarPhotos CP WITH(NOLOCK) ON CP.AbSure_CarDetailsId = ACD.Id
	--WHERE ACD.DealerId  = @DealerId and ACD.Id NOT IN(SELECT AbSure_CarDetailsId FROM AbSure_CarPhotos WHERE ImageCaption LIKE '%RC%' OR (ImageTagType=2 AND ImageTagId=1))
	
END
