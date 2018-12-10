IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeliveryNoteDetailsSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeliveryNoteDetailsSave]
GO

	-- =============================================  
-- Author:  Surendra  
-- Create date: 19th Oct,2011  
-- Description: This procedure will return all the details to print receipt
-- Modified By: Nilesh Utture on 26 Nov 2012 at 3 pm : Added @EngineNumber parameter 
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DeliveryNoteDetailsSave]  
(  
@TC_BookingDelivery_Id INT ,  
@DeliveryNotes VARCHAR(400),  
@ChassisNumber VARCHAR(30),  
@LincenseNumber VARCHAR(30),  
@EngineNumber VARCHAR(30)  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
 UPDATE TC_BookingDelivery SET DeliveryNotes=@DeliveryNotes,  
 ChassisNumber=@ChassisNumber,LincenseNumber=@LincenseNumber,   
 EngineNumber = @EngineNumber  
 WHERE TC_BookingDelivery_Id=@TC_BookingDelivery_Id   
END

SET ANSI_NULLS ON
