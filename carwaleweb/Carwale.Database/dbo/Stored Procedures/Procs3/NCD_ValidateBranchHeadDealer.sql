IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_ValidateBranchHeadDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_ValidateBranchHeadDealer]
GO

	
-- =============================================
-- Author:		<Author : Vinay Kumar Prajapati>
-- Create date: <21/02/2014>
-- Description:	<This SP validate dealer which is associate with  HeadBranch.
-- =============================================
CREATE PROCEDURE [dbo].[NCD_ValidateBranchHeadDealer]
(  
 @DealerId INT ,  
 @IsValidDealer BIT OUTPUT 
)
AS
   
BEGIN  
	SELECT HBD.DealerId FROM NCD_HeadBranchDealers HBD	WHERE HBD.DealerId=@DealerId
	
	
  IF (@@ROWCOUNT <> 0)  
	  BEGIN  
		   SET @IsValidDealer = 1     
	  END 
  ELSE
  BEGIN 
     SET @IsValidDealer = 0   
  END 

END