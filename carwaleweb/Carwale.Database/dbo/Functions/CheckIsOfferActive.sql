IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckIsOfferActive]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CheckIsOfferActive]
GO

	


--([dbo].[GetIsActive]([status]))
---- Created By Vinayak on 25-01-2014 
CREATE FUNCTION [dbo].[CheckIsOfferActive]
(@StartDate DATETIME,@EndDate DATETIME)
 
RETURNS BIT
AS

BEGIN

	

	DECLARE @CurDate DATETIME
	
	DECLARE @RetBit bit = 0
	
	SET @CurDate = CONVERT(DATE,GETDATE())

	IF (@CurDate >= @StartDate)
	   BEGIN
	      IF (@CurDate <= @EndDate)
			BEGIN
			   SET @RetBit = 1
			END
	   END
	
	RETURN @RetBit

END;





