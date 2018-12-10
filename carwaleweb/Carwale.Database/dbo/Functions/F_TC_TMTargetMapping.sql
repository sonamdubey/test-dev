IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[F_TC_TMTargetMapping]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[F_TC_TMTargetMapping]
GO

	-- ============================================= 
-- Author:   Vivek Singh
-- Create date: 13-12-13
-- Description:  Return the Final Target of respective Target type based on Retail Target
-- ============================================= 
CREATE FUNCTION [dbo].[F_TC_TMTargetMapping](@RetailTarget     INT, 
                                             @TargetType      INT
                                            )
											
  RETURNS FLOAT
	AS 
    BEGIN 
	DECLARE @TargetValue int
	IF(@TargetType=1)--Total inquiries
	  
	    SET @TargetValue=@RetailTarget * 7
	   	
	ELSE IF(@TargetType=2)--Test Drive

	    SET @TargetValue=@RetailTarget * 5.6
	
	ELSE IF(@TargetType=3)--Booking
	
	    SET @TargetValue=@RetailTarget * 1.1
	
	ELSE IF(@TargetType=5)--Delivery
	
	    SET @TargetValue=@RetailTarget
	
	ELSE IF(@TargetType=6)--Finance

	    SET  @TargetValue=@RetailTarget* 0.55
	
	ELSE IF(@TargetType=7)--VW finance
	
	    SET @TargetValue=@RetailTarget * 0.25
	 
	ELSE IF(@TargetType=8)--Insurance
	
	    SET @TargetValue=@RetailTarget * 0.8
	
	ELSE IF(@TargetType=9)--Trade-In
	
	    SET @TargetValue=@RetailTarget * 0.3

	ELSE
	    SET @TargetValue=@RetailTarget
	
 RETURN
 @TargetValue
 END
