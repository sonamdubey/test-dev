IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AddSubscription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AddSubscription]
GO

	CREATE PROCEDURE [dbo].[AddSubscription]
	@EmailAddress         VARCHAR(255),
	@SubscriptionCategory TINYINT,
	@SubscriptionType     BIGINT, 
	@Frequency            VARCHAR(20), 
	@SubscriptionDate     DATE, 
	@Status               BIT OUTPUT 
AS 
  BEGIN 
      DECLARE @SubscriptionID INT 
      DECLARE @Freq VARCHAR(20) 

      SET @SubscriptionID = (SELECT SubscriptionID 
                             FROM   Subscription 
                             WHERE  EmailAddress = @EmailAddress 
                                    AND SubscriptionCategory = 
                                        @SubscriptionCategory 
                                    AND SubscriptionType = @SubscriptionType) 

      IF ( @SubscriptionID IS NULL ) 
        BEGIN 
            INSERT INTO dbo.Subscription (EmailAddress, SubscriptionCategory, SubscriptionType, Frequency, SubscriptionDate)
            VALUES      (@EmailAddress, 
                         @SubscriptionCategory, 
                         @SubscriptionType, 
                         @Frequency, 
                         @SubscriptionDate) 

            SET @Status = 1 
        END 
      ELSE 
        BEGIN 
            SET @Freq = (SELECT Frequency 
                         FROM   Subscription 
                         WHERE  SubscriptionID = @SubscriptionID) 

            IF ( @Freq != @Frequency ) 
              BEGIN 
                  UPDATE Subscription 
                  SET    Frequency = @Frequency 
                  WHERE  SubscriptionID = @SubscriptionID 

                  SET @Status = 1 
              END 
            ELSE 
              BEGIN 
                  SET @Status = 0 
              END 
        END
  END 