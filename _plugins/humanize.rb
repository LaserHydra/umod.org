class String
  def humanize
    # TODO: Add support for other string formats and small words (ie. and, to, or, of)

    # List of abbreviations and/or words to exclude from formatting
    exclusions = [ "IPv4", "IPv6", "MySQL" ]

    # Split at upper case letter prefixed by an excluded word or a lower case letter  (ex. MySQLWhitelist or WelcomeTP)
    self.gsub(/(!#{exclusions.join('|')}|[[:lower:]\\d])([[:upper:]])/, '\1 \2') \
    # Split at upper case letter prefixed by two or more upper case letters           (ex. NPCManager or NPCKitC4)
    .gsub(/([[:upper:]]{2,})([[:upper:]][[:lower:]\\d])/, '\1 \2') \
    # Split at upper case letter prefixed by two or more numbers                      (ex. Epic99Arena)
    .gsub(/(\d{2,})(\d+)/, '\1 \2') \
    # Split at upper case letter prefixed by a number                                 (ex. C4Logger)
    .gsub(/(\d)/, '\1 \2') \
    # Remove any whitespace from start or end of string
    .strip
  end
end

module Jekyll
  module HumanizeFilter
    def humanize(input)
      input.humanize
    end
  end
end

Liquid::Template.register_filter(Jekyll::HumanizeFilter)
