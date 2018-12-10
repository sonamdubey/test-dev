IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQProcessOther]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQProcessOther]
GO

	-- =============================================
-- Author:		Tejashree Patil	
-- Create date: 17 Jan 2013
-- Description:	To process inquiries from dealer website
-- Modiifed By : Tejashree Patil on 14 Feb 2013,Removed unused parameters(@AutoVerified) and implemented try catch.
-- Modiifed By : Tejashree Patil on 13 May 2013,Added condition @InquiryTypeId > 5 for Other inquiry.
-- for more inquiry type added for skoda website
-- Modified By : Umesh on 26 June 2013 for service request inquiry will also go in other inquiries table
-- Modified By : Tejashree Patil on 30 Jun 2014, PreferedDateTime is inserted in TC_OtherRequest.
-- Modified By : Tejashree Patil on 9 July 2014, PreferedDateTime datatype is changed from date to datetime.
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQProcessOther] 
	 @DealerId BIGINT,  
	 @InquiryTypeId TINYINT,  
	 @CustomerName VARCHAR(100), 
	 @CustomerEmail VARCHAR(100), 
	 @CustomerMobile VARCHAR(15),
	 @InquirySourceId TINYINT, 
	--Specific to ContactRequest,ServiceRequest
	 @Comments VARCHAR(250) = NULL,
	---Specific to ServiceRequest
	 @VersionId INT = NULL,  
	 @RegistrationNo VARCHAR(20) = NULL,  
	 @PreferedDate DATETIME = NULL,  
	 @TypeOfService SMALLINT = NULL,
	 @Status TINYINT OUTPUT
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION ProcessOtherInquiries
	--Registering/Updating Customer
		DECLARE @CustomerId BIGINT
		DECLARE @CustStatus SMALLINT
		DECLARE @ActiveLeadId BIGINT

		EXEC TC_CustomerDetailSave @BranchId=@DealerId,@CustomerEmail=@CustomerEmail,@CustomerName=@CustomerName,@CustomerMobile=@CustomerMobile,
			@Location=NULL,@Buytime=NULL,@Comments=NULL,@CreatedBy=NULL,@Address=NULL,@SourceId=@InquirySourceId,
			@CustomerId=@CustomerId OUTPUT,@Status=@CustStatus OUTPUT,@ActiveLeadId=@ActiveLeadId OUTPUT,@CW_CustomerId=NULL--@CW_CustomerId
			
		IF(@CustStatus=1) --Customer Is Fake
		BEGIN
			SET @Status=0		
		END
		ELSE
		BEGIN				
			-- 5:Service Request
			IF(@InquiryTypeId = 5)
			BEGIN			
				INSERT INTO TC_ServiceRequests(TC_InquiriesId,RegNo,PreferredDate,TypeOfService,Comments,VersionId,TC_CustomerId,InquirySourceId,CreatedDate) 
				VALUES(NULL, @RegistrationNo, @PreferedDate, @TypeOfService, @Comments,@VersionId,@CustomerId,@InquirySourceId,GETDATE())	
				
			END
			
			-- 8:Finance Inquiry(Loan) , 10:Grievance Redressal		,	
			IF(@InquiryTypeId > 4)		
			BEGIN
				INSERT INTO TC_OtherRequests ( TC_InquiriesId,Comments,TC_InquiryTypeId,TC_CustomerId,InquirySourceId,CreatedDate,PreferredDateTime)
				VALUES(NULL,@Comments, @InquiryTypeId,@CustomerId,@InquirySourceId,GETDATE(),@PreferedDate)	--Modified By : Tejashree Patil on 30 Jun 2014.	
				
			END
						
			SET @Status=1
		END
		COMMIT TRANSACTION ProcessOtherInquiries		
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION ProcessOtherInquiries
		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,InputParameters)
         VALUES('TC_INQProcessOther',(ERROR_MESSAGE()+' ERROR_NUMBER(): '+ERROR_NUMBER()),GETDATE(),
         (	' @DealerId:'+ CAST(@DealerId AS VARCHAR(50))+' @VersionId:'+  CAST(ISNULL(@VersionId,'') AS VARCHAR(50))+
			' @InquiryTypeId:'+   CAST(@InquiryTypeId AS VARCHAR(5)) +' @CustomerName:'+  @CustomerName+
			' @Email:'+  @CustomerEmail+' @Mobile:'+  @CustomerMobile +' @InquirySourceId:'+  CAST(@InquirySourceId AS VARCHAR(50))+
			' @RegistrationNo:'+  CAST(ISNULL(@RegistrationNo ,'') AS VARCHAR(50))+ ' @PreferedDate : '+ CAST(ISNULL(@PreferedDate,'')  AS VARCHAR(50)) + 
			' @TypeOfService : ' + CAST(ISNULL(@TypeOfService,'') AS VARCHAR(50))))
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END




/****** Object:  StoredProcedure [dbo].[TC_OtherInqListLoad]    Script Date: 7/7/2014 2:34:22 PM ******/
SET ANSI_NULLS ON
