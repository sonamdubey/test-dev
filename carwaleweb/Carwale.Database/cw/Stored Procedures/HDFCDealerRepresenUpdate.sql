IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[HDFCDealerRepresenUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[HDFCDealerRepresenUpdate]
GO

	-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [cw].[HDFCDealerRepresenUpdate]    
 -- Add the parameters for the stored procedure here    
   
 @Name varchar(50),    
 @mobile varchar(10),    
 @Email varchar(100),    
 @EntryDate datetime,    
 @IsActive bit,   
 @DealerId numeric(18),
 @ASM varchar(50),      
 @ASMContactNo varchar(10),      
 @ASMEmailId varchar(100)        
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 UPDATE dbo.HDFCDealerRepresentatives   
 SET Representative=@Name,Mobile=@mobile,Email=@Email,EntryDate=@EntryDate,IsActive=@IsActive ,ASM=@ASM,ASMEmailId=@ASMEmailId,ASMContactNo=@ASMContactNo  
 WHERE DealerId=@DealerId;    
END 