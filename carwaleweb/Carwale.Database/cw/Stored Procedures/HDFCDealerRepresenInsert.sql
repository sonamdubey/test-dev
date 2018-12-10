IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[HDFCDealerRepresenInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[HDFCDealerRepresenInsert]
GO

	-- ================================================    
-- Template generated from Template Explorer using:    
-- Create Procedure (New Menu).SQL    
--    
-- Use the Specify Values for Template Parameters     
-- command (Ctrl-Shift-M) to fill in the parameter     
-- values below.    
--    
-- This block of comments will not be included in    
-- the definition of the procedure.    
-- ================================================    
    
-- =============================================    
-- Author:  <Author,,Name>    
-- Create date: <Create Date,,>    
-- Description: <Description    
CREATE PROCEDURE [cw].[HDFCDealerRepresenInsert]    
 -- Add the parameters for the stored procedure here    
 @DealerId numeric(18),    
 @Name varchar(50),    
 @mobile varchar(10),    
 @Email varchar(100),    
 @EntryDate datetime,    
 @IsActive bit=1,
 @ASM varchar(50),    
 @ASMContactNo varchar(10),    
 @ASMEmailId varchar(100)       
     
     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    Insert into dbo.HDFCDealerRepresentatives(DealerId,Representative,Mobile,Email,EntryDate,isActive,ASM,ASMEmailId,ASMContactNo) values(@DealerId,@Name,@mobile,@Email,@EntryDate,@IsActive,@ASM,@ASMEmailId,@ASMContactNo);    
      
END 