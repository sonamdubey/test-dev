IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DeleteTesitmonial]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DeleteTesitmonial]
GO

	-- =============================================  
-- Author:  SURENDRA  
-- Create date: 11th Aug,2011  
-- Description: Updating NCD_Photos  
-- =============================================  
CREATE PROCEDURE [dbo].[Microsite_DeleteTesitmonial]  
(  
 @Id int  
)  
AS  
BEGIN  
 --SET NOCOUNT ON;  
 UPDATE Microsite_Testimonials SET IsActive=0 WHERE Id =@Id  
END 