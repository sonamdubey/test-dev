IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCD_InsertInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCD_InsertInquiry]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 15th July 2011
-- Description:	This proc insert data in NCD_Inquiries table
-- Modifier :	Ruchira Patil on 17th April 2014 (updating the DelLeads and DailyDel coulmn of NCD_Dealers table)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NCD_InsertInquiry]
	
	(
	@CustomerId		VARCHAR(2000),
	@Name			VARCHAR(2000),
	@Email			VARCHAR(2000),
	@Mobile			VARCHAR(2000),
	@CityId			VARCHAR(2000),
	@VersionID		VARCHAR(2000),
	@BuyPlan		VARCHAR(2000),
	@DealerId		NUMERIC
	)
	AS
	BEGIN
	
	
			DECLARE @custidIndx SMALLINT,@strId VARCHAR(10),@nameIndx SMALLINT,@strName VARCHAR(200),@emailIndx SMALLINT,
					@strEmail VARCHAR(50),@mobileIndx SMALLINT,@strMobile VARCHAR(15),@cityIndx SMALLINT,@strCity VARCHAR(10),
					@versionIndx SMALLINT,@strVersion VARCHAR(10),@buyPlanIndx SMALLINT,@strBuyPlan VARCHAR(50),@Customer INT,
					@CampaignId  INT,@DailyDel INT ,@DelLeads INT ,@DailyCount INT ,@TargetLeads INT


			SELECT @CampaignId = CampaignId FROM NCD_Dealers WHERE DealerId = @DealerId

			WHILE @CustomerId <> ''                 
				BEGIN                
						SET @custidIndx = CHARINDEX(',', @CustomerId) 
						SET @nameIndx = CHARINDEX(',', @Name) 
						SET @emailIndx = CHARINDEX(',', @Email) 
						SET @mobileIndx = CHARINDEX(',', @Mobile) 
						SET @cityIndx = CHARINDEX(',', @CityId) 
						SET @versionIndx = CHARINDEX(',', @VersionID) 
						SET @buyPlanIndx = CHARINDEX(',', @BuyPlan) 
				
				--to check if list id has ended                
						IF @custidindx>0                
							BEGIN  
            
								SET @strId = LEFT(@CustomerId, @custidIndx-1)  
								SET @CustomerId = RIGHT(@CustomerId, LEN(@CustomerId)-@custidIndx) 
                
								SET @strName = LEFT(@Name, @nameIndx-1)   
								SET @Name = RIGHT(@Name, LEN(@Name)-@nameIndx) 
				                
								SET @strEmail = LEFT(@Email, @emailIndx-1)  
								SET @Email = RIGHT(@Email, LEN(@Email)-@emailIndx)
				                
								SET @strMobile = LEFT(@Mobile, @mobileIndx-1)  
								SET @Mobile = RIGHT(@Mobile, LEN(@Mobile)-@mobileIndx)
				                
								SET @strCity = LEFT(@CityId, @cityIndx-1)   
								SET @CityId = RIGHT(@CityId, LEN(@CityId)-@cityIndx)
				                
								SET @strVersion = LEFT(@VersionID, @versionIndx-1)   
								SET @VersionID = RIGHT(@VersionID, LEN(@VersionID)-@versionIndx)
				                
								SET @strBuyPlan = LEFT(@BuyPlan, @buyPlanIndx-1)   
								SET @BuyPlan = RIGHT(@BuyPlan, LEN(@BuyPlan)-@buyPlanIndx)
				                
				                SELECT @Customer = ID FROM ncd_customers where Email=@strEmail
				                
								IF @@ROWCOUNT = 0
									BEGIN
									
										INSERT  ncd_customers (Name,email,Mobile,EntryDate,CustomerSource,IsActive)
										values(@strName,@strEmail,@strMobile,GETDATE(),2,1)
										
										SET	@Customer = SCOPE_IDENTITY()
										 
									END
								
								--Insert Into NCD_Inquiries
								--(
								--	DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
								--)
								--Values
								--(
								--	@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
								--)

								IF @CampaignId > 0
								BEGIN
									SELECT @DailyDel = DailyDel,@DelLeads = DelLeads,@DailyCount = DailyCount,@TargetLeads = TargetLeads 
									FROM NCD_Dealers
									WHERE DealerId = @DealerId

									IF @TargetLeads > ISNULL(@DelLeads,0)
									BEGIN
										IF ISNULL(@DailyCount,0) > 0
										BEGIN
											IF @DailyCount > ISNULL(@DailyDel,0)
											BEGIN
												Insert Into NCD_Inquiries
												(
													DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
												)
												Values
												(
													@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
												)

												UPDATE NCD_Dealers SET DailyDel = ISNULL(DailyDel,0) + 1 , DelLeads = ISNULL(DelLeads,0) + 1 WHERE  DealerId = @DealerId
											END
										END
										ELSE
										BEGIN
											Insert Into NCD_Inquiries
											(
												DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
											)
											Values
											(
												@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
											)

											UPDATE NCD_Dealers SET DailyDel = ISNULL(DailyDel,0) + 1 , DelLeads = ISNULL(DelLeads,0) + 1 WHERE  DealerId = @DealerId
										END
									END
								END
							END                
						ELSE                
							BEGIN                
                
								SET @strId = @CustomerId  
								SET @CustomerId = ''   
				                
								SET @strName = @Name
								SET @strEmail =@Email
								SET @strMobile = @Mobile
								SET @strCity = @CityId
								SET @strVersion = @VersionID
								SET @strBuyPlan = @BuyPlan   
								
								SELECT @Customer = ID FROM ncd_customers where Email=@strEmail
								IF @@ROWCOUNT = 0
									BEGIN
										insert into ncd_customers (Name,email,Mobile,EntryDate,CustomerSource,IsActive)
										values(@strName,@strEmail,@strMobile,GETDATE(),2,1)
									   set	@Customer = @@IDENTITY 
									END
								
								--Insert Into NCD_Inquiries
								--(
								--	DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
								--)
								--Values
								--(
								--	@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
								--)

								IF @CampaignId > 0
								BEGIN
									SELECT @DailyDel = DailyDel,@DelLeads = DelLeads,@DailyCount = DailyCount,@TargetLeads = TargetLeads 
									FROM NCD_Dealers
									WHERE DealerId = @DealerId

									IF @TargetLeads > ISNULL(@DelLeads,0)
									BEGIN
										IF ISNULL(@DailyCount,0) > 0
										BEGIN
											IF @DailyCount > ISNULL(@DailyDel,0)
											BEGIN
												Insert Into NCD_Inquiries
												(
													DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
												)
												Values
												(
													@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
												)

												UPDATE NCD_Dealers SET DailyDel = ISNULL(DailyDel,0) + 1 , DelLeads = ISNULL(DelLeads,0) + 1 WHERE  DealerId = @DealerId
											END
										END
										ELSE
										BEGIN
											Insert Into NCD_Inquiries
											(
												DealerId,CustomerId,CityId,VersionId,CampaignId,EntryDate,InquirySource,IsActive,BuyPlan,RequestType, CWCustomerId
											)
											Values
											(
												@DealerId,@Customer,@strCity,@strVersion,@CampaignId,GETDATE(),2,1,@strBuyPlan,1, @strId
											)

											UPDATE NCD_Dealers SET DailyDel = ISNULL(DailyDel,0) + 1 , DelLeads = ISNULL(DelLeads,0) + 1 WHERE  DealerId = @DealerId
										END
									END
								END
							 END         
				END
END
						
							
	
	
	

