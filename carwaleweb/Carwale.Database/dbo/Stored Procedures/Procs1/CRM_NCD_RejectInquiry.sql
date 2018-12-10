IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCD_RejectInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCD_RejectInquiry]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 21st July 2011
-- Description:	This proc insert data in NCD_RejectedLeads table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NCD_RejectInquiry]
	
	
	@CustomerId		VARCHAR(2000),
	@InquiryDate	VARCHAR(3000)
	
	AS
	BEGIN
	
	
			DECLARE @custidIndx SMALLINT,@strId VARCHAR(10),@dateIndx SMALLINT,@strdate VARCHAR(2000)  
  
			WHILE @CustomerId <> ''                 
				BEGIN                
						SET @custidIndx = CHARINDEX(',', @CustomerId) 
						SET @dateIndx = CHARINDEX(',', @InquiryDate) 
						
				
				--to check if list id has ended                
						IF @custidindx>0                
							BEGIN  
            
								SET @strId = LEFT(@CustomerId, @custidIndx-1)  
								SET @CustomerId = RIGHT(@CustomerId, LEN(@CustomerId)-@custidIndx) 
                
								SET @strdate = LEFT(@InquiryDate, @dateIndx-1)   
								SET @InquiryDate = RIGHT(@InquiryDate, LEN(@InquiryDate)-@dateIndx) 
				                
								 
								
								Insert Into NCD_RejectedLeads
								(
									CustomerId,InquiryDate
								)
								Values
								(
									@strId,convert(datetime,@strdate)
								)
							END                
						ELSE                
							BEGIN                
                
								SET @strId = @CustomerId  
								SET @CustomerId = ''   
				                SET @strdate = @InquiryDate
								  
								Insert Into NCD_RejectedLeads
								(
									CustomerId,InquiryDate
								)
								Values
								(
									@strId,convert(datetime,@strdate)
								)
								
							 END         
				END
END
						
							
	
	
	
