-- AddPlayer errors
EXEC sp_addmessage 
    @msgnum = 50001, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Nickname already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50002, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Email already used', -- Error message
    @lang = 'us_english'; -- Language
GO;

EXEC sp_addmessage 
    @msgnum = 50003, -- Unique error number (must be >50000 for user-defined messages)
    @severity = 16,  -- user-defined errors
    @msgtext = 'Elo must be between 0 and 3000', -- Error message
    @lang = 'us_english'; -- Language
GO;