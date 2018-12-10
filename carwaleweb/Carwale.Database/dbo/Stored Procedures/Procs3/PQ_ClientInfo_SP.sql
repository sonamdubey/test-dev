IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_ClientInfo_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_ClientInfo_SP]
GO

	/*  
 Procedure Name: PQ_ClientInfo_SP  
 Procedure Description: Save the Inquiry Id And related Information for a Price quote  
    Created By: Vikas   
    Created On: 04-Apr-2012  
	-- modified by ashish Verma setting clientIp to Null on 27/08/2014
*/  
CREATE Procedure [dbo].[PQ_ClientInfo_SP]  
@PQId Numeric,  
@AspNetSessionId VarChar(100),  
@ClientIP VarChar(100) = NULL,  -- modified by ashish Verma setting clientIp to Null
@EntryDate DateTime ,
@CWCookieValue VarChar(100) 
As  
Begin  
 Insert Into PQ_ClientInfo (PQId, AspNetSessionId, ClientIP, EntryDate, CWCookieValue) 
 Values (@PQId, @AspNetSessionId, @ClientIP, @EntryDate, @CWCookieValue)   
End
