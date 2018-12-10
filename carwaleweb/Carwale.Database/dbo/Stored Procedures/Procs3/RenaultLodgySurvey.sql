IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[RenaultLodgySurvey]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[RenaultLodgySurvey]
GO

	-- =============================================
-- Author:		Mihir A chheda
-- Create date: 06-04-2015
-- Description:	Stores the customer survey result for Family Car of 2015  Renault Lodgy
-- Modeified by : Kritika Choudhary on 25th June 2015, added parameters @CityId and @ContactNumber
-- =============================================
CREATE PROCEDURE [dbo].[RenaultLodgySurvey]
	-- Add the parameters for the stored procedure here
@CustomerId Int OUTPUT,
@CustName Varchar(50),
@CustEmail Varchar(50),
@Source Varchar(50),
@CityId Int,
@ContactNumber Varchar(10),
@QuestionIds Varchar(100),
@OptionIds  Varchar(100)

AS
BEGIN
 IF(@CustomerId=-1)
BEGIN
	INSERT INTO CW_RenaultLodgyCustomer 
    (Name,Email,Source,CityId,ContactNumber) 
	VALUES(@CustName,@CustEmail,@Source,@CityId,@ContactNumber)
	SET @CustomerId=SCOPE_IDENTITY()
END
ELSE IF(@CustomerId!=-1)
BEGIN
	DECLARE @Quespos INT
	DECLARE @Anspos INT
	DECLARE @Queslen INT
	DECLARE @Anslen INT
	DECLARE @Questionvalue varchar(100)
	DECLARE @Answervalue varchar(100)

	set @Quespos = 0
	set @Queslen = 0
	set @Anspos = 0
	set @Anslen = 0

   WHILE CHARINDEX(',', @QuestionIds, @Quespos+1)>0
   BEGIN
    set @Queslen = CHARINDEX(',', @QuestionIds, @Quespos+1) - @Quespos
	SET @Anslen = CHARINDEX(',', @OptionIds, @Anspos+1) - @Anspos

    set @Questionvalue = SUBSTRING(@QuestionIds, @Quespos, @Queslen)
	SET @Answervalue= SUBSTRING(@OptionIds, @Anspos, @Anslen)
    
--	PRINT @Questionvalue +'-'+@Answervalue

	INSERT INTO CW_RenaultLodgyCustomerAnswers 
	(CustomerId,QuestionId,OptionId) 
	VALUES(@CustomerId,@Questionvalue,@Answervalue)
   
    set @Quespos = CHARINDEX(',', @QuestionIds, @Quespos+@Queslen) +1
	set @Anspos = CHARINDEX(',', @OptionIds, @Anspos+@Anslen) +1
END
END
END
