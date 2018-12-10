IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CalculateHerfindahlIndex]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CalculateHerfindahlIndex]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <01-11-2012>
-- Description:	<Calculates Herfindahl Index>
--Modified By Reshma Shetty 27/08/2013 Calculate Herfindahl only if @Total is > 0 otherwise return 0 
-- =============================================
CREATE FUNCTION [dbo].[CalculateHerfindahlIndex] 
(
	-- Add the parameters for the function here
	@C1 BIGINT,
	@C2 BIGINT,
	@C3 BIGINT,
	@C4 BIGINT,
	@C5 BIGINT
)
RETURNS BIGINT
AS
BEGIN
	-- Declare the return variable here -- @HFLIndex BIGINT=0
	DECLARE @Total FLOAT,@HFLIndex FLOAT=0,@Temp FLOAT
	
	SET @Total=ISNULL(@C1,0)+ISNULL(@C2,0)+ISNULL(@C3,0)+ISNULL(@C4,0)+ISNULL(@C5,0)--+ISNULL(@C6,0)

	IF (@Total>0) --Modified By Reshma Shetty 27/08/2013 Calculate Herfindahl only if @Total is > 0 otherwise return 0 

		BEGIN
		SET @Temp=(ISNULL(@C1,0)*ISNULL(@C1,0)+ISNULL(@C2,0)*ISNULL(@C2,0)+ISNULL(@C3,0)*ISNULL(@C3,0)+
				   ISNULL(@C4,0)*ISNULL(@C4,0)+ISNULL(@C5,0)*ISNULL(@C5,0))--+ISNULL(@C6,0)*ISNULL(@C6,0))
				   /(@Total*@Total)
		
		SET @HFLIndex= CASE WHEN @Temp<=0.4 THEN 1 
							WHEN @Temp<=0.5 THEN 2 
							WHEN @Temp<=0.8 THEN 3 
							WHEN @Temp>0.8 THEN 4  END
		END
    RETURN @HFLIndex
END
