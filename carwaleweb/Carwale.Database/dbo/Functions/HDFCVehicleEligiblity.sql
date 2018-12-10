IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[HDFCVehicleEligiblity]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[HDFCVehicleEligiblity]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <29/08/2012>
-- Description:	<Returns the eligibility of a stock based on
--                   1. Car is from the HDFC-empaneled dealers' list 
--                   2. Not more than 5 years old 
--                   3. Up to 2 previous owners 
--                   4. Label price not less than Rs.1,00,000 >
--                   SELECT dbo.HDFCVehicleEligiblity (473,10000000,1,4)
-- Modified:Reshma Shetty 26/09/2012 Make Year criteria changed from 5(previously) to 8(currently) years 
-- Modified by : Manish on 22-04-2014 added WITH(NOLOCK) keyword wherever not found.
-- =============================================
CREATE FUNCTION [dbo].[HDFCVehicleEligiblity] 
(
	-- Add the parameters for the function here
	@DealerId	INT,
	@Price BIGINT,
	@Owners	TINYINT,
	@MakeYear SMALLINT
)
RETURNS BIT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Bit BIT = 0

	-- Add the T-SQL statements to compute the return value here
	/*SELECT @Bit = 1 
	FROM TC_Stock TS WITH(NOLOCK) 
		  INNER JOIN HDFCDealerRepresentatives HDR WITH(NOLOCK)  ON HDR.DealerId=TS.BranchId
		  INNER JOIN TC_CarCondition AS TC WITH(NOLOCK)  ON TC.StockId=TS.Id
	WHERE TS.Id = @StockId 
		  AND TS.Price >= 100000
		  AND TC.Owners <= 2  
		  AND DATEDIFF(Year,TS.MakeYear,GETDATE()) < 5 
	*/
	
	SELECT @Bit = 1 FROM HDFCDealerRepresentatives WITH(NOLOCK) WHERE DealerId = @DealerId AND IsActive=1 AND @Price >= 100000 AND @Owners <= 2 AND @MakeYear < 8
		
	-- Return the result of the function
	RETURN @Bit

END
