#!/bin/bash
# Usage: ./extract_labels.sh <music file>
# Output: (in current directory)
#   <filename>_stats.json   - full description and statistics from Essentia
#   <filename>_beats.txt    - beat positions, in seconds, separated with a ', ' (comma and a space)
#   <filename>_labels.txt   - beat labels in a format suitable for loading with Audacity

INPUT="$1"
BASE_NAME=`basename "$INPUT"`
STATS="${BASE_NAME}_stats.json"
BEATS="${BASE_NAME}_beats.txt"
LABELS="${BASE_NAME}_labels.txt"

./streaming_extractor_music "$INPUT" "$STATS"
grep 'beats_position' "$STATS" | sed -E 's/(.*\[)(.*)(\].*)/\2/;s/(, )/\n/g' > "$BEATS"
sed -E 's/(.+)/\1\t\1\t*/' "$BEATS" > "$LABELS"
