IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetRejectionReasons]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AbSure_GetRejectionReasons]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: May 19,2015
-- Description:	To get comma separated list of rejection reasons
-- SELECT dbo.[AbSure_GetRejectionReasons](360)
-- =============================================
CREATE FUNCTION [dbo].[AbSure_GetRejectionReasons]
(
	@Absure_CarDetailsId BIGINT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @Reasons VARCHAR(MAX) = NULL, @ReasonsIds VARCHAR(MAX) = NULL
	/*

    SELECT	@Reasons = (COALESCE(@Reasons + ', ', '') +  CAST(ARR.Reason AS VARCHAR(500))) 
    FROM    AbSure_RejectionReasons ARR
	WHERE   ARR.Id IN(SELECT ListMember FROM [dbo].[fnSplitCSV_WithId] (@ReasonsIds))   
	
	RETURN	@Reasons
	*/
	
	
	--DECLARE	@ReasonsId	VARCHAR(MAX) = NULL
	--DECLARE @Reasons	VARCHAR(MAX) = NULL

	SELECT		@Reasons = (COALESCE(@Reasons + ', ', '') +  
				CAST((CASE ARCR.RejectedType WHEN 1 THEN ARR.Reason ELSE AQC.Category + '-' + AQS.SubCategory END) AS VARCHAR(500)))
	FROM		Absure_RejectedCarReasons ARCR	WITH(NOLOCK)
	LEFT JOIN	AbSure_CarDetails ACD			WITH(NOLOCK) ON ACD.Id = ARCR.Absure_CarDetailsId
	LEFT JOIN	AbSure_RejectionReasons ARR		WITH(NOLOCK) ON ARR.Id = ARCR.RejectedReason AND ARCR.RejectedType=1
	LEFT JOIN	AbSure_QSubCategory AQS			WITH(NOLOCK) ON AQS.AbSure_QSubCategoryId = ARCR.RejectedReason AND ARCR.RejectedType=2
	LEFT JOIN	AbSure_QCategory AQC			WITH(NOLOCK) ON AQC.AbSure_QCategoryId = AQS.AbSure_QCategoryId
	WHERE		ARCR.Absure_CarDetailsId = @Absure_CarDetailsId

 --   SELECT	@ReasonsIds = (COALESCE(@ReasonsIds + ', ', '') +  CAST(ARCR.RejectedReason AS VARCHAR(500))) 
 --   FROM    Absure_RejectedCarReasons ARCR WITH(NOLOCK)
	--WHERE   ARCR.Absure_CarDetailsId = @Absure_CarDetailsId
	----SELECT  @ReasonsId 'ReasonsIds'
	
 --   SELECT	@Reasons = (COALESCE(@Reasons + ', ', '') +  CAST(ARR.Reason AS VARCHAR(500))) 
 --   FROM    AbSure_RejectionReasons ARR WITH(NOLOCK)
	--WHERE   ARR.Id IN (SELECT ListMember FROM [dbo].[fnSplitCSV_WithId] (@ReasonsIds))  
	
	RETURN  @Reasons
	

END

