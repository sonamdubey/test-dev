IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_IsCarOnHold]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_IsCarOnHold]
GO

	
-- =============================================
-- Author		:	Yuga Hatolkar
-- Create date	:    8th Oct, 2015
-- Description	:	To check if car is on hold or not.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_IsCarOnHold] 

@AbSure_CarDetailsId BIGINT,
@IsOnHold BIT OUTPUT

AS
BEGIN
DECLARE
	@Status	INT	= 0
	,@DoubtfulReason INT = 0
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;   

    SELECT @Status = ACD.Status, @DoubtfulReason = DoubtfulReason 
	FROM   AbSure_CarDetails ACD WITH(NOLOCK) 
	LEFT JOIN AbSure_DoubtfulCarReasons ADC WITH(NOLOCK) ON ACD.Id = ADC.AbSure_CarDetailsId
	WHERE ACD.Id = @AbSure_CarDetailsId	AND ADC.DoubtfulReason NOT IN (3,4)

	IF  @Status = 9 
	BEGIN	
		SET @IsOnHold = 1	
	END
	ELSE
	BEGIN	
		SET @IsOnHold = 0
	END

END
---------------------------------------------------------------