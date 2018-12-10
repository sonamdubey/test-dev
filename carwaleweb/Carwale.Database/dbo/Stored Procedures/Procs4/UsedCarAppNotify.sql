IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarAppNotify]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarAppNotify]
GO

	
-- Author		:	Sahil & Afrose
-- Create date	:	21/04/2016 14:00 PM
-- Description	:	This SP will create used car app notification data
--exec UsedCarAppNotify_v1
-- =============================================   
CREATE PROCEDURE [dbo].[UsedCarAppNotify]	
AS
BEGIN

SET NOCOUNT ON

--Type1 Notification Code Starts Here: Notification to users who have given response on a used car through ios/android app
DECLARE @LeadLookUpSpan SMALLINT, @NotificationResendSpan SMALLINT, @DetailPageLookUpSpan SMALLINT, @row int;


SELECT 
    @LeadLookUpSpan         = MAX(CASE WHEN Id = 2 THEN Span END),
    @NotificationResendSpan = MAX(CASE WHEN Id = 3 THEN Span END),
	@DetailPageLookUpSpan  =  MAX(CASE WHEN Id = 4 THEN Span END)
FROM UsedCarNotificationConfig WITH(NOLOCK)
WHERE 
    Id IN(2, 3, 4);

DECLARE @LeadLookUpStartDate DATETIME = DATEADD(DAY,@LeadLookUpSpan,GETDATE());
DECLARE @NotificationResendDuration DATETIME = DATEADD(DAY,@NotificationResendSpan,GETDATE());
DECLARE @DetailPageLookupDuration DATETIME = DATEADD(DAY,@DetailPageLookUpSpan,GETDATE());



--To fetch last response given by particular ImeiCode
--Fetching response from UsedCarPurchaseInquiries and ClassifiedRequests table and partioning them
--based on IMEICode and ordering so that latest RequestDateTime is at the top of each partition.
--Dumping data to temp table #LatestIMEIResponse
SELECT UsedResponseUnion.SellInquiryId,UsedResponseUnion.RequestDateTime as LastResponseDateTime, 
UsedResponseUnion.SourceId, UsedResponseUnion.IMEICode,UsedResponseUnion.SellerType, 
ROW_NUMBER() OVER(PARTITION BY UsedResponseUnion.IMEICode ORDER BY UsedResponseUnion.RequestDateTime DESC) AS ROWNUM
INTO #LatestIMEIResponse 
FROM(
        --To fetch leads coming from android/ios in last 30 days
        SELECT SellInquiryId, RequestDateTime,SourceId, IMEICode, 1 AS SellerType 
        FROM UsedCarPurchaseInquiries WITH(NOLOCK)
        WHERE RequestDateTime >= @LeadLookUpStartDate AND IMEICode IS NOT NULL AND SourceId IN(74,83)
        UNION
        SELECT SellInquiryId, RequestDateTime,SourceId, IMEICode, 2 AS SellerType 
        FROM ClassifiedRequests WITH(NOLOCK)
        WHERE RequestDateTime >= @LeadLookUpStartDate AND IMEICode IS NOT NULL AND SourceId IN(74,83)
    ) UsedResponseUnion;

	SET @row=@@ROWCOUNT
	
IF(@row>0) 
BEGIN 
		--Filter out ImeiCodes that have received notification in past 7 days and user with used car notification set off.Dumping data into temp table #EligibleForNotification
		SELECT 
		   IDENTITY(INT, 1, 1) AS Id, 
		   EligibleForNotification.SellInquiryId,
		   EligibleForNotification.LastResponseDateTime,
		   EligibleForNotification.SourceId,
		   EligibleForNotification.IMEICode,
		   EligibleForNotification.MobileUserId,
		   EligibleForNotification.GCMRegId,
		   EligibleForNotification.LastNotified,
		   EligibleForNotification.SubsMasterId,
		   EligibleForNotification.SellerType
		   INTO #EligibleForNotification 
		   FROM(
					SELECT LatestIMEIResponse.SellInquiryId,
						   LatestIMEIResponse.LastResponseDateTime, 
						   CASE LatestIMEIResponse.SourceId WHEN 74 THEN 0 WHEN 83 THEN 1 END AS SourceId,
						   LatestIMEIResponse.IMEICode,
						   LatestIMEIResponse.SellerType, 
						   MobileUsers.MobileUserId,
						   MobileUsers.GCMRegId, 
						   UserSubscriptionMapping.SubsMasterId, 
						   UserSubscriptionMapping.IsActive,
						   NotificationLog.LastNotified						  
					FROM #LatestIMEIResponse LatestIMEIResponse WITH(NOLOCK)
					INNER JOIN  Mobile.MobileUsers MobileUsers WITH(NOLOCK) ON LatestIMEIResponse.IMEICode = MobileUsers.IMEICode
					INNER JOIN Mobile.UserSubscriptionMapping UserSubscriptionMapping WITH(NOLOCK) ON MobileUsers.MobileUserId =  UserSubscriptionMapping.MobileUserId
					LEFT JOIN UsedCarAppNotificationLog NotificationLog WITH(NOLOCK) ON NotificationLog.IMEICode=LatestIMEIResponse.IMEICode
					WHERE LatestIMEIResponse.ROWNUM=1 
					AND UserSubscriptionMapping.SubsMasterId = 8 
					AND UserSubscriptionMapping.IsActive=1
					AND (NotificationLog.LastNotified IS NULL
						 OR
						 NotificationLog.LastNotified < @NotificationResendDuration
					    )
			) EligibleForNotification;				
			

		--Date by which look up will start for new car added with particular city+make+root
		DECLARE @StartDate DATETIME;

		DECLARE @InitialLoopValue INT=1, @FinalLoopValue INT, @RootId SMALLINT, @MakeId NUMERIC, @CityId NUMERIC, 
		@CountOfCarsUpdated INT, @Url VARCHAR(500), @MakeName VARCHAR(100), @RootName VARCHAR(100), 
		@CityName VARCHAR(100), @Content VARCHAR(500), @SellInquiryId INT, @SellerType SMALLINT,@imei varchar(20)

		SELECT @FinalLoopValue= Count(Id) FROM #EligibleForNotification WITH(NOLOCK)		

		--Iterate each row of #EligibleForNotification table and select if any new listing added
		--Check live listing exists and new listing added of same make,root and city as user had shown interest on.
		--Check if new car added after Max(LastNotificationSent,LastResponseTime).  
		WHILE(@InitialLoopValue <= @FinalLoopValue)
		BEGIN
		   --If first time notification case
		   IF ((SELECT LastNotified from #EligibleForNotification WITH(NOLOCK) WHERE Id=@InitialLoopValue) is null)
		   BEGIN
		        
				 SELECT @StartDate=LastResponseDateTime 
				 FROM #EligibleForNotification WITH(NOLOCK) 
				 WHERE id=@InitialLoopValue;	
							
		   END
		   ELSE --Notified earlier: Check maximum b/w last notified and lastResponseTime
		   BEGIN
				SELECT @StartDate = CASE WHEN LastResponseDateTime>LastNotified 
								    THEN LastResponseDateTime 
								    ELSE LastNotified 
									END
									FROM #EligibleForNotification WITH(NOLOCK) 
									WHERE id=@InitialLoopValue;	
									 		
		   END

		   SET @RootId=NULL;SET @MakeId=NULL;SET @CityId=NULL;SET @MakeName=NULL;SET @RootName=NULL;SET @CityName=NULL;SET @SellInquiryId=NULL;SET @imei=NULL;

		   --Fetch sellInquiryId, SellerType, Imei of customer eligible for notification
		   SELECT @SellInquiryId = SellInquiryId, @SellerType = SellerType, @imei=IMEICode 
		   FROM #EligibleForNotification WITH(NOLOCK) 
		   WHERE id=@InitialLoopValue;

		   --Check if car exists in livelisting on which user had shown last interest (IF not exist its SOLD OUT CASE: The car would be deleted from livelisting after users lead) 
		   IF EXISTS (SELECT 1 FROM livelistings WITH(NOLOCK) WHERE Inquiryid=@SellInquiryId AND SellerType=@SellerType)
		   BEGIN
		      	--Fetch necessary parameters from live listing to create url and title to be sent to app notification table.
				SELECT @RootId =RTRIM(LTRIM(CONVERT(VARCHAR,ROOTID))), 
					   @MakeId=RTRIM(LTRIM(CONVERT(VARCHAR,MakeId))), 
					   @CityId=RTRIM(LTRIM(CONVERT(VARCHAR,CityId))),
				       @MakeName=MakeName, 
					   @RootName=RootName, 
					   @CityName=CityName
				FROM LIVELISTINGS WITH(NOLOCK)
				WHERE LIVELISTINGS.InquiryId = @SellInquiryId
				      AND LIVELISTINGS.SellerType = @SellerType;

				--Get count of cars added on which user had shown last interest.
			   SELECT @CountOfCarsUpdated = COUNT(Inquiryid)
			   FROM LIVELISTINGS WITH(NOLOCK) 
			   WHERE ROOTID=@RootId 
					 AND MakeId=@MakeId 
					 AND CityId=@CityId 
					 AND EntryDate > @StartDate					 			

			   --If we get some car that was added after user was last notified or he had shown interest.
			   --Create Url and Title and dump it to the app notification table - 'UsedCarAppNotification'
			   IF (@CountOfCarsUpdated > 0)
			   BEGIN
					SELECT @Url='webapi/classified/stock/?car='+CONVERT(Varchar,@MakeId)+'.'+CONVERT(Varchar,@RootId)+'&city='+CONVERT(Varchar,@CityId) + '&so=1&sc=6';
					SELECT @Content='Checkout newly added '+ISNULL(@MakeName,'')+' '+ISNULL(@RootName,'')+' in '+ ISNULL(@CityName,'')
										
					BEGIN TRY
					  INSERT INTO UsedCarAppNotification(GCMRegId, Content, Url, OSType, NotificationType,UsedCarNotificationId,IMEICode, Title,EntryDate)		
					  SELECT GCMRegId,@Content,@Url,SourceId,SubsMasterId,1,IMEICode,'CarWale',GETDATE() 
					  FROM #EligibleForNotification WITH(NOLOCK) 
					 WHERE Id=@InitialLoopValue
					 select 0;
					END TRY
					BEGIN CATCH
					INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car App Notification Type 1',
									        'dbo.UsedCarAppNotify',
											 ERROR_MESSAGE(),
											 'UsedCarAppNotification',
											 @InitialLoopValue,
											 GETDATE()
                                            )
					END CATCH	
			   END	
		   END
		   SET @InitialLoopValue  = @InitialLoopValue  + 1        
		END

		DROP TABLE #EligibleForNotification;
END

DROP TABLE #LatestIMEIResponse;
--Type1 Notification Code Ends Here: Notification to users who have given response on a used car through ios/android app




--Type2 Notification Code Starts Here: Notification to users who have not given response(Type1) but have viewed used car detail page through ios/android app

--Remove app users who have already been shortlisted to get type1 notification (users who gave response)
--Pick users only which have viewed car details page through app within @DetailPageLookupDuration.
--Temp table #ViewedDetailNotConverted: Users who viewed car details page on app but did not give response.
SELECT IMEICode,ProfileId,UsedCarNotificationId,SourceId,EntryTime,ROW_NUMBER() OVER(PARTITION BY UsedDetailViewLog.IMEICode ORDER BY UsedDetailViewLog.EntryTime DESC) AS RNUM 
INTO #ViewedDetailNotConverted
FROM UsedCarDetailViewLog UsedDetailViewLog WITH(NOLOCK)
WHERE SourceId IN(74,83)
AND IMEICode IS NOT NULL 
AND EntryTime >= @DetailPageLookupDuration
AND NOT EXISTS (SELECT 1 FROM UsedCarAppNotification WITH(NOLOCK) WHERE UsedCarAppNotification.IMEICode = UsedDetailViewLog.IMEICode)

--Remove app users which have turn off used car app notification Or have been notified already within @NotificationResendDuration
--Extract GCMRegId of the IMEI, LastNotified time of user
SELECT  IDENTITY(INT, 1, 1) AS Id, 
           ViewedDetailNotConverted.ProfileId,
		   ViewedDetailNotConverted.EntryTime as LastDetailViewTime,
		   CASE ViewedDetailNotConverted.SourceId WHEN 74 THEN 0 WHEN 83 THEN 1 END AS SourceId,
		   ViewedDetailNotConverted.IMEICode,
		   UserSubscriptionMapping.SubsMasterId,
		   MobileUsers.GCMRegId,
		   NotificationLog.LastNotified	  
INTO #EligibleForDetailViewNotification
FROM #ViewedDetailNotConverted ViewedDetailNotConverted WITH(NOLOCK)
INNER JOIN  Mobile.MobileUsers MobileUsers WITH(NOLOCK) ON ViewedDetailNotConverted.IMEICode = MobileUsers.IMEICode
INNER JOIN Mobile.UserSubscriptionMapping UserSubscriptionMapping WITH(NOLOCK) ON MobileUsers.MobileUserId =  UserSubscriptionMapping.MobileUserId
LEFT JOIN UsedCarAppNotificationLog NotificationLog WITH(NOLOCK) ON NotificationLog.IMEICode=ViewedDetailNotConverted.IMEICode
WHERE ViewedDetailNotConverted.RNUM=1 
AND UserSubscriptionMapping.SubsMasterId = 8 
AND UserSubscriptionMapping.IsActive=1
AND (NotificationLog.LastNotified IS NULL
	OR
	NotificationLog.LastNotified < @NotificationResendDuration
)

--Re initialize the variables with default values.
SELECT @StartDate = NULL, @InitialLoopValue=1, @FinalLoopValue = NULL, @RootId = NULL, @MakeId = NULL, 
@CityId = NULL, @CountOfCarsUpdated = NULL, @Url = NULL, @MakeName = NULL, @RootName = NULL, 
@CityName = NULL, @Content = NULL, @imei = NULL;

SELECT @FinalLoopValue= Count(Id) FROM #EligibleForDetailViewNotification WITH(NOLOCK)		
	
	--Iterate the rows of app users who are eligible for details page view notification (Type2 notification)
	WHILE(@InitialLoopValue <= @FinalLoopValue)
	BEGIN
			   --If first time notification case
			   IF ((SELECT LastNotified from #EligibleForDetailViewNotification WITH(NOLOCK) WHERE Id=@InitialLoopValue) is null)
			   BEGIN
					
					 SELECT @StartDate=LastDetailViewTime 
					 FROM #EligibleForDetailViewNotification WITH(NOLOCK) 
					 WHERE Id=@InitialLoopValue;	
								
			   END
			   ELSE --Notified earlier: Check maximum b/w last notified and LastDetailViewTime
			   BEGIN
					SELECT @StartDate = CASE WHEN LastDetailViewTime>LastNotified 
										THEN LastDetailViewTime 
										ELSE LastNotified 
										END
										FROM #EligibleForDetailViewNotification WITH(NOLOCK) 
										WHERE id=@InitialLoopValue;	
												
			   END

			   DECLARE @ProfileId VARCHAR(50);
			   SELECT @RootId=NULL,@MakeId=NULL,@CityId=NULL,@MakeName=NULL,@RootName=NULL,@CityName=NULL,@imei=NULL,@ProfileId=NULL;

			   --Check if @imei is needed here
			   --Fetch sellInquiryId, SellerType, Imei of customer eligible for notification
			   SELECT @ProfileId = RTRIM(LTRIM(UPPER(ProfileId))), @imei=IMEICode 
			   FROM #EligibleForDetailViewNotification WITH(NOLOCK) 
			   WHERE id=@InitialLoopValue;

			   --Check if car exists in livelisting on which user had shown last interest (IF not exist its SOLD OUT CASE: The car would be deleted from livelisting after users lead) 
			   IF EXISTS (SELECT 1 FROM livelistings WITH(NOLOCK) WHERE RTRIM(LTRIM(UPPER(ProfileId)))=@ProfileId)
			   BEGIN
		      		--Fetch necessary parameters from live listing to create url and title to be sent to app notification table.
					SELECT @RootId =RTRIM(LTRIM(CONVERT(VARCHAR,ROOTID))), 
						   @MakeId=RTRIM(LTRIM(CONVERT(VARCHAR,MakeId))), 
						   @CityId=RTRIM(LTRIM(CONVERT(VARCHAR,CityId))),
						   @MakeName=MakeName, 
						   @RootName=RootName, 
						   @CityName=CityName
					FROM LIVELISTINGS WITH(NOLOCK)
					WHERE RTRIM(LTRIM(UPPER(LIVELISTINGS.ProfileId))) = @ProfileId;

					--Get count of cars added on which user had shown last interest.
				   SELECT @CountOfCarsUpdated = COUNT(Inquiryid)
				   FROM LIVELISTINGS WITH(NOLOCK) 
				   WHERE ROOTID=@RootId 
						 AND MakeId=@MakeId 
						 AND CityId=@CityId 
						 AND EntryDate > @StartDate					 			

				   --If we get some car that was added after user was last notified or he had shown interest.
				   --Create Url and Title and dump it to the app notification table - 'UsedCarAppNotification'
				   IF (@CountOfCarsUpdated > 0)
				   BEGIN
						SELECT @Url='webapi/classified/stock/?car='+CONVERT(Varchar,@MakeId)+'.'+CONVERT(Varchar,@RootId)+'&city='+CONVERT(Varchar,@CityId) + '&so=1&sc=6';
						SELECT @Content='Checkout newly added '+ISNULL(@MakeName,'')+' '+ISNULL(@RootName,'')+' in '+ ISNULL(@CityName,'')
										
						BEGIN TRY
						  INSERT INTO UsedCarAppNotification(GCMRegId, Content, Url, OSType, NotificationType,UsedCarNotificationId,IMEICode, Title,EntryDate)		
						  SELECT GCMRegId,@Content,@Url,SourceId,SubsMasterId,2,IMEICode,'CarWale',GETDATE() 
						  FROM #EligibleForDetailViewNotification WITH(NOLOCK) 
						  WHERE Id=@InitialLoopValue
						END TRY
						BEGIN CATCH
						INSERT INTO CarWaleWebSiteExceptions (
												ModuleName,
												SPName,
												ErrorMsg,
												TableName,
												FailedId,
												CreatedOn)
										VALUES ('Used Car App Notification Type2 ',
												'dbo.UsedCarAppNotify',
												 ERROR_MESSAGE(),
												 'UsedCarAppNotification',
												 @InitialLoopValue,
												 GETDATE()
												)
						END CATCH	
				   END	
			   END
			   SET @InitialLoopValue  = @InitialLoopValue  + 1        
	END


DROP TABLE #ViewedDetailNotConverted;
DROP TABLE #EligibleForDetailViewNotification;
--Type2 Notification Code Starts Here: Notification to users who have not given response(Type1) but have viewed used car detail page through ios/android app

END

