IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQPQMailerSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQPQMailerSave]
GO

	-- =============================================    
-- Author:  Tejashree Patil    
-- Create date: 7 January 2013 at 12.30 pm    
-- Description: To save price quote request   
-- Modified By: Nilesh Utture on 24th jan, 2013 Added PqCompletedDate in Insert query
-- EXEC [TC_INQPQMailerSave] 1,'ads','asda','as@as.in','Nilesh','asd.pdf',1,5
-- =============================================    
CREATE PROCEDURE [dbo].[TC_INQPQMailerSave]
	 @TC_NewCarInquiriesId INT,	 
	 @Subject VARCHAR,    
	 @MailBody VARCHAR,
	 @CustomerEmail VARCHAR(100),
	 @MailBy VARCHAR,
	 @MailFile VARCHAR, 
	 @TC_UsersId INT,         
	 @BranchId BIGINT
AS    
BEGIN   
	DECLARE @LeadId BIGINT
	DECLARE @InquiriesLeadId BIGINT
	
	SELECT @InquiriesLeadId = TC_InquiriesLeadId FROM TC_NewCarInquiries WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
	SELECT @LeadId = TC_LeadId FROM TC_InquiriesLead 
	WHERE TC_InquiriesLeadId = @InquiriesLeadId
	
	IF NOT EXISTS(SELECT TOP 1 TC_PQId FROM TC_PqRequest WHERE TC_NewCarInquiriesId=@TC_NewCarInquiriesId )
	BEGIN
	    
		INSERT INTO  TC_PqRequest(TC_NewCarInquiriesId,RequestedDate,PqCompletedDate,PqStatus,TC_UsersId)    
		VALUES		(@TC_NewCarInquiriesId,GETDATE(),GETDATE(),25,@TC_UsersId)    
	  
		UPDATE		 TC_NewCarInquiries
		SET			 PQRequestedDate=GETDATE(),PQDate=GETDATE(),PQStatus=25
		WHERE		 TC_NewCarInquiriesId=@TC_NewCarInquiriesId

		EXEC TC_DispositionLogInsert @TC_UsersId,25,@TC_NewCarInquiriesId,5,@LeadId
	  
	END 	  
	ELSE    
	BEGIN  
		UPDATE		TC_PqRequest SET PqStatus=25,LastUpdatedDate=GETDATE(),PqCompletedDate=GETDATE()
		WHERE		TC_NewCarInquiriesId=@TC_NewCarInquiriesId		
		
		EXEC TC_DispositionLogInsert @TC_UsersId,25,@TC_NewCarInquiriesId,5,@LeadId
				
	END  
	
	DECLARE @TC_PQId BIGINT
	SELECT	@TC_PQId=TC_PQId 
	FROM	TC_PqRequest WITH(NOLOCK)
	WHERE	TC_NewCarInquiriesId=@TC_NewCarInquiriesId
	
	INSERT INTO TC_MailData(Subject, MailBody, CustomerEmail, MailBy, MailFile, MailDate, TC_PQId) 
	VALUES(@Subject,@MailBody, @CustomerEmail, @MailBy, @MailFile, GETDATE(), @TC_PQId)
	  
END
