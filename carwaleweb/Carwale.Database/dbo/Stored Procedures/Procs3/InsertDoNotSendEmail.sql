IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertDoNotSendEmail]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertDoNotSendEmail]
GO

	
-- =============================================  
-- Author:  Manish  
-- Create date: 11-Feb-2014 
-- Details: SP will insert the record in DoNotSendEmail
-- =============================================  
CREATE PROCEDURE [dbo].[InsertDoNotSendEmail] @Email AS VARCHAR(100)
 AS           
  BEGIN 
       INSERT INTO DoNotSendEmail (Email
					               )
							VALUES (@Email
								   )
	
					             
								   
   END 