IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_VerifyMobile_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_VerifyMobile_15]
GO

	

--Modified by Aditi Dhaybar 0n 13/10/14. 
--Modified by Prachi Phalak on 24/08/2015
--Modified by Prachi Phalak on 12.09.2015  Added a condition for inquiryid and Seller type in the insert query
--Modified by Manish on 14-09-2015 added try and catch block.
--Modified by Akansha on 15-09-2015 added condition @inquiryId <> ''
CREATE PROCEDURE [dbo].[CV_VerifyMobile_15.9.2]
	@EmailId 		AS VARCHAR(100), 
	@MobileNo 		AS VARCHAR(50), 
	@CVID			AS VARCHAR(100), 
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@EntryDateTime	AS DATETIME,
	@SourceId		AS TINYINT,				--Modified by Aditi Dhaybar 0n 13/10/14.
	@inquiryId      AS VARCHAR(100) = Null,
	@CustName       AS VARCHAR(50) = Null,	
	@SellerType		AS VARCHAR(50) = Null,	
	@IsMobileVer	AS BIT OUTPUT,
	@NewCVID		AS NUMERIC OUTPUT
	
	
AS

BEGIN

       BEGIN TRY

	SELECT * FROM CV_MobileEmailPair WITH (NOLOCK)         --Modified by Aditi Dhaybar 0n 13/10/14. Added WITH (NOLOCK) 
	WHERE EmailId = @EmailId AND MobileNo = @MobileNo

	IF @@ROWCOUNT <> 0
		BEGIN
			SET @IsMobileVer = 1
		END
	ELSE
		BEGIN
			SET @IsMobileVer = 0
		END

	IF @IsMobileVer = 0
		BEGIN
			IF @CVID = -1
				BEGIN
					EXEC [dbo].[CV_InsertPendingList_14.10.1] @CWICode, @CUICode, @EmailId, @MobileNo, @EntryDateTime, @SourceId, @NewCVID OUTPUT		--Modified by Aditi Dhaybar 0n 13/10/14. Added SourceId
					if(@inquiryId IS NOT NULL AND @inquiryId <> ''
					        AND NOT EXISTS(SELECT 1 
							                 FROM ClassifiedLeads WITH (NOLOCK) 
											    WHERE CustMobile = @MobileNo  
												 AND  CustEmail = @EmailId 
												 AND InquiryId = @inquiryId 
												 AND sellerType = @SellerType)
										    )
						BEGIN
							INSERT INTO ClassifiedLeads (InquiryId,CustName,CustEmail,CustMobile,CarwaleInternalClientId,SellerType)
							values(@InquiryId,@CustName,@EmailId,@MobileNo,@SourceId,@SellerType);
						END
				END
		END

		END TRY
		BEGIN CATCH
		 INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Mobile Verification',
									        'dbo.CV_VerifyMobile_15.9.2',
											 ERROR_MESSAGE(),
											 'ClassifiedLeads',
											 'SourceId:'+CONVERT(VARCHAR,@SourceId),
											 GETDATE()
                                            )

		END CATCH ;
		
END

